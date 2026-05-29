# AWS credential vault – IAM setup

## Problem: CreateSecret AccessDenied after adding IAM user policy

`secretsmanager:CreateSecret` is authorized against a resource that **does not exist yet**.  
IAM user policies that only list secret ARNs (e.g. `secret:*/*`) often **do not match** and CreateSecret still fails.

## 1. IAM user policy (identity)

User: **`fsg-storage-service`**

Apply [`iam-fgs-credentials-secrets-policy.json`](iam-fgs-credentials-secrets-policy.json):

```powershell
aws iam put-user-policy `
  --user-name fsg-storage-service `
  --policy-name FgsCredentialsSecretsManager `
  --policy-document file://deployment/aws/iam-fgs-credentials-secrets-policy.json
```

Important: **`CreateSecret` uses `"Resource": "*"`** in that file.

## 2. KMS key policy (required for CMK encryption)

IAM on the user is not enough. The **KMS key** used in `AwsCredentials:KmsKeyArn` must allow:

1. IAM user `fsg-storage-service`
2. Service `secretsmanager.amazonaws.com` (via `kms:ViaService`)

Merge statements from [`kms-key-policy-addon-fsg-storage-service.json`](kms-key-policy-addon-fsg-storage-service.json) into the key policy in AWS Console:

**KMS → Customer managed keys → your key → Key policy → Edit**

Or CLI (gets current policy, you must merge manually):

```powershell
aws kms get-key-policy `
  --key-id arn:aws:kms:us-east-1:286093098927:key/8ad55556-fcb0-4dd7-8ed1-4de526a38a78 `
  --policy-name default `
  --output text
```

## 3. Verify caller and permissions

```powershell
$env:AWS_ACCESS_KEY_ID = "<AccessKeyId from AwsCredentials>"
$env:AWS_SECRET_ACCESS_KEY = "<SecretAccessKey>"
$env:AWS_REGION = "us-east-1"

aws sts get-caller-identity

aws iam simulate-principal-policy `
  --policy-source-arn arn:aws:iam::286093098927:user/fsg-storage-service `
  --action-names secretsmanager:CreateSecret `
  --resource-arns "*"
```

Expected: `"EvalDecision": "allowed"` for CreateSecret.

Test create (optional):

```powershell
aws secretsmanager create-secret `
  --name "dev/fsm/test-tenant/postgres" `
  --secret-string '{"server":"localhost","database":"postgres","username":"postgres","password":"postgres"}' `
  --kms-key-id arn:aws:kms:us-east-1:286093098927:key/8ad55556-fcb0-4dd7-8ed1-4de526a38a78 `
  --region us-east-1
```

If CLI succeeds but the app fails, restart the API so it reloads appsettings.

## 4. Common mistakes

| Issue | Fix |
|-------|-----|
| Policy attached to wrong user | Confirm `aws sts get-caller-identity` shows `fsg-storage-service` |
| Old inline policy still has `secret:*/*` only | Replace with updated JSON (`CreateSecret` on `*`) |
| KMS key policy missing user | Add KMS key policy statements (step 2) |
| IAM propagation delay | Wait 1–2 minutes after policy change |
| API not restarted | Stop debug session and restart after appsettings/IAM changes |
