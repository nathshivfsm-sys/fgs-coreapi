# Attach FgsS3AllBuckets policy (all S3 buckets) to an IAM user or role.
#
# Usage (from repo root):
#   .\deployment\aws\Attach-FgsS3AllBucketsPolicy.ps1 -UserName fsg-storage-service
#   .\deployment\aws\Attach-FgsS3AllBucketsPolicy.ps1 -RoleName fgs-dev-ec2-role
#   .\deployment\aws\Attach-FgsS3AllBucketsPolicy.ps1 -UserName fsg-storage-service -KmsKeyArn "arn:aws:kms:us-east-1:ACCOUNT:key/KEY-ID"
#
# Requires: AWS CLI v2 + credentials with iam:PutUserPolicy / iam:PutRolePolicy

param(
    [Parameter(ParameterSetName = "User")]
    [string]$UserName,

    [Parameter(ParameterSetName = "Role")]
    [string]$RoleName,

    [string]$PolicyName = "FgsS3AllBuckets",

    [string]$KmsKeyArn = "",

    [string]$Region = "us-east-1"
)

$ErrorActionPreference = "Stop"

if (-not $UserName -and -not $RoleName) {
    throw "Specify -UserName or -RoleName."
}

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$PolicyFile = Join-Path $ScriptDir "iam-fgs-s3-all-buckets-policy.json"

if (-not (Test-Path $PolicyFile)) {
    throw "Missing policy file: $PolicyFile"
}

$aws = Get-Command aws -ErrorAction SilentlyContinue
if (-not $aws) {
    $candidates = @(
        "$env:ProgramFiles\Amazon\AWSCLIV2\aws.exe",
        "$env:LOCALAPPDATA\Programs\Amazon\AWSCLIV2\aws.exe"
    )
    foreach ($c in $candidates) {
        if (Test-Path $c) {
            $env:Path = "$(Split-Path $c);$env:Path"
            break
        }
    }
}

Write-Host "Caller identity:"
aws sts get-caller-identity --region $Region
Write-Host ""

if ($UserName) {
    Write-Host "Attaching $PolicyName to user '$UserName'..."
    aws iam put-user-policy `
        --user-name $UserName `
        --policy-name $PolicyName `
        --policy-document "file://$PolicyFile"
    Write-Host "OK: user policy attached."
}
else {
    Write-Host "Attaching $PolicyName to role '$RoleName'..."
    aws iam put-role-policy `
        --role-name $RoleName `
        --policy-name $PolicyName `
        --policy-document "file://$PolicyFile"
    Write-Host "OK: role policy attached."
}

if ($KmsKeyArn) {
    $kmsPolicyName = "FgsS3KmsForBuckets"
    $kmsDoc = @{
        Version = "2012-10-17"
        Statement = @(
            @{
                Sid    = "KmsForS3"
                Effect = "Allow"
                Action = @(
                    "kms:Encrypt"
                    "kms:Decrypt"
                    "kms:GenerateDataKey"
                    "kms:DescribeKey"
                    "kms:CreateGrant"
                )
                Resource = @($KmsKeyArn)
            }
        )
    } | ConvertTo-Json -Depth 6 -Compress

    $tmp = [IO.Path]::GetTempFileName()
    try {
        Set-Content -Path $tmp -Value $kmsDoc -Encoding utf8NoBOM
        if ($UserName) {
            Write-Host "Attaching $kmsPolicyName to user '$UserName' for $KmsKeyArn..."
            aws iam put-user-policy `
                --user-name $UserName `
                --policy-name $kmsPolicyName `
                --policy-document "file://$tmp"
        }
        else {
            Write-Host "Attaching $kmsPolicyName to role '$RoleName' for $KmsKeyArn..."
            aws iam put-role-policy `
                --role-name $RoleName `
                --policy-name $kmsPolicyName `
                --policy-document "file://$tmp"
        }
        Write-Host "OK: KMS policy attached."
    }
    finally {
        Remove-Item $tmp -ErrorAction SilentlyContinue
    }
}

Write-Host ""
Write-Host "Next:"
Write-Host "  1. Wait ~30-60s for IAM propagation"
Write-Host "  2. Restart file-service on EC2:"
Write-Host "       sudo /opt/fgs/deploy-service.sh file-service dev"
Write-Host "  3. Retry attachment upload"
