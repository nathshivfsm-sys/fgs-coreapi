"""
One-off DEV seed: DATADOG provider type + Global GloCredential row.
Reads DB/AWS from Setup appsettings.Development.json and ApiKey from Gateway/.env.
"""
from __future__ import annotations

import json
import os
import struct
import sys
from pathlib import Path

try:
    import boto3
    import psycopg
    from cryptography.hazmat.primitives.ciphers.aead import AESGCM
except ImportError:
    print("Installing boto3, psycopg, cryptography...", flush=True)
    import subprocess

    subprocess.check_call(
        [sys.executable, "-m", "pip", "install", "--quiet", "boto3", "psycopg[binary]", "cryptography"]
    )
    import boto3
    import psycopg
    from cryptography.hazmat.primitives.ciphers.aead import AESGCM


ROOT = Path(__file__).resolve().parents[1]
APPSETTINGS = ROOT / "src" / "SetupService" / "Fgs.Setup.API" / "appsettings.Development.json"
GATEWAY_ENV = ROOT / "src" / "Gateway" / ".env"


def load_json(path: Path) -> dict:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def load_dotenv(path: Path) -> dict[str, str]:
    values: dict[str, str] = {}
    if not path.exists():
        return values
    for line in path.read_text(encoding="utf-8").splitlines():
        line = line.strip()
        if not line or line.startswith("#") or "=" not in line:
            continue
        key, value = line.split("=", 1)
        values[key.strip()] = value.strip().strip('"').strip("'")
    return values


def parse_npgsql(cs: str) -> dict[str, str]:
    parts: dict[str, str] = {}
    for segment in cs.split(";"):
        segment = segment.strip()
        if not segment or "=" not in segment:
            continue
        k, v = segment.split("=", 1)
        parts[k.strip()] = v.strip()
    return parts


def encrypt_payload(plaintext: bytes, plaintext_key: bytes) -> bytes:
    """Match AesGcmEncryptionService: [version:1][nonce:12][tag:16][ciphertext]."""
    nonce = os.urandom(12)
    aesgcm = AESGCM(plaintext_key)
    # cryptography returns ciphertext||tag
    sealed = aesgcm.encrypt(nonce, plaintext, None)
    ciphertext, tag = sealed[:-16], sealed[-16:]
    return bytes([1]) + nonce + tag + ciphertext


def main() -> int:
    app = load_json(APPSETTINGS)
    env = load_dotenv(GATEWAY_ENV)
    api_key = env.get("DD_API_KEY") or os.environ.get("DD_API_KEY")
    site = env.get("DD_SITE") or os.environ.get("DD_SITE") or "datadoghq.com"
    if not api_key:
        print("DD_API_KEY missing from Gateway/.env", file=sys.stderr)
        return 1

    aws = app["AwsCredentials"]
    cs = parse_npgsql(app["ConnectionStrings"]["FgsSetup"])
    conninfo = (
        f"host={cs['Host']} port={cs.get('Port', '5432')} dbname={cs['Database']} "
        f"user={cs['Username']} password={cs['Password']}"
    )

    payload = json.dumps({"ApiKey": api_key, "Site": site}, separators=(",", ":")).encode("utf-8")

    kms = boto3.client(
        "kms",
        region_name=aws["Region"],
        aws_access_key_id=aws["AccessKeyId"],
        aws_secret_access_key=aws["SecretAccessKey"],
    )
    data_key = kms.generate_data_key(KeyId=aws["KmsKeyArn"], KeySpec="AES_256")
    plaintext_key = data_key["Plaintext"]
    encrypted_key = data_key["CiphertextBlob"]
    credential_data = encrypt_payload(payload, plaintext_key)

    provider_sql = """
    INSERT INTO glo."GloCredentialProviderType"
    (
        "ProviderCode", "ProviderName", "ConfigurationSchema", "IsActive", "CreatedOn", "CreatedBy"
    )
    SELECT
        'DATADOG',
        'Datadog',
        '[
            {"key":"ApiKey","label":"API Key","type":"password","required":true,"sensitive":true},
            {"key":"Site","label":"Site (e.g. datadoghq.com)","type":"text","required":false}
        ]'::jsonb,
        TRUE,
        timezone('utc', now()),
        'SYSTEM'
    WHERE NOT EXISTS (
        SELECT 1 FROM glo."GloCredentialProviderType" t WHERE t."ProviderCode" = 'DATADOG'
    );

    INSERT INTO setup."GloCredentialProviderTypeCache"
    (
        "ProviderTypeId", "ProviderCode", "ProviderName", "ConfigurationSchema", "IsActive", "UpdatedOn"
    )
    SELECT
        src."Id", src."ProviderCode", src."ProviderName", src."ConfigurationSchema", src."IsActive", timezone('utc', now())
    FROM glo."GloCredentialProviderType" src
    WHERE src."ProviderCode" = 'DATADOG'
      AND NOT EXISTS (
          SELECT 1 FROM setup."GloCredentialProviderTypeCache" c
          WHERE c."ProviderCode" = src."ProviderCode"
      );

    UPDATE setup."GloCredentialProviderTypeCache" AS c SET
        "ProviderTypeId" = src."Id",
        "ProviderName" = src."ProviderName",
        "ConfigurationSchema" = src."ConfigurationSchema",
        "IsActive" = src."IsActive",
        "UpdatedOn" = timezone('utc', now())
    FROM glo."GloCredentialProviderType" AS src
    WHERE src."ProviderCode" = 'DATADOG'
      AND c."ProviderCode" = src."ProviderCode";
    """

    with psycopg.connect(conninfo) as conn:
        conn.execute(provider_sql)
        provider_id = conn.execute(
            """
            SELECT "Id" FROM glo."GloCredentialProviderType"
            WHERE "ProviderCode" = 'DATADOG'
            """
        ).fetchone()[0]

        existing = conn.execute(
            """
            SELECT "Id" FROM glo."GloCredential"
            WHERE "CredentialProviderTypeId" = %s
              AND "CredentialName" = 'DatadogDev'
            """,
            (provider_id,),
        ).fetchone()

        if existing:
            conn.execute(
                """
                UPDATE glo."GloCredential"
                SET "CredentialData" = %s,
                    "EncryptedDataKey" = %s,
                    "Description" = %s,
                    "IsActive" = TRUE,
                    "UpdatedOn" = timezone('utc', now()),
                    "UpdatedBy" = 'SYSTEM'
                WHERE "Id" = %s
                """,
                (credential_data, encrypted_key, "Datadog API key for local/dev", existing[0]),
            )
            credential_id = existing[0]
            action = "updated"
        else:
            credential_id = conn.execute(
                """
                INSERT INTO glo."GloCredential"
                (
                    "CredentialProviderTypeId",
                    "CredentialName",
                    "Description",
                    "CredentialData",
                    "EncryptedDataKey",
                    "IsActive",
                    "CreatedOn",
                    "CreatedBy"
                )
                VALUES
                (
                    %s, 'DatadogDev', 'Datadog API key for local/dev',
                    %s, %s, TRUE, timezone('utc', now()), 'SYSTEM'
                )
                RETURNING "Id"
                """,
                (provider_id, credential_data, encrypted_key),
            ).fetchone()[0]
            action = "created"

        conn.commit()

    print(f"DATADOG provider ready. Global credential '{action}' id={credential_id}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
