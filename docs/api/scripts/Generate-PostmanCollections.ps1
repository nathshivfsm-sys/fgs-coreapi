<#
.SYNOPSIS
    Generates Postman v2.1 collections from FGS API controllers.
#>
param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path,
    [string]$OutputDir = (Join-Path $RepoRoot "docs\api")
)

$ErrorActionPreference = "Stop"

function New-PostmanUuid { return [guid]::NewGuid().ToString() }

function Get-RouteTemplate {
    param([string]$Raw, [string]$ControllerName)
    if ($Raw -eq '[controller]') {
        return ($ControllerName -replace 'Controller$','').ToLowerInvariant()
    }
    return $Raw.Trim().Trim('/')
}

function Test-AllowAnonymous {
    param([string]$ClassBlock, [string]$MethodBlock)
    return ($ClassBlock -match '\[AllowAnonymous\]') -or ($MethodBlock -match '\[AllowAnonymous\]')
}

function Test-RequiresAuth {
    param([string]$ClassHeader, [string]$MethodBlock)
    if ($MethodBlock -match '\[AllowAnonymous\]') { return $false }
    if ($ClassHeader -match '(?m)^\s*\[AllowAnonymous\]') { return $false }
    return $true
}

function Get-PostmanVarName {
    param([string]$ParamName)
    switch ($ParamName) {
        'id' { 'recordId' }
        default { $ParamName }
    }
}

function Convert-RouteToPostmanPath {
    param([string]$Template)
    if ([string]::IsNullOrWhiteSpace($Template)) { return '' }
    return [regex]::Replace($Template, '\{(\w+)(?::(?:long|guid))?\}', {
        param($m)
        $varName = Get-PostmanVarName $m.Groups[1].Value
        '{' + '{' + $varName + '}' + '}'
    })
}

function Get-HttpVerb {
    param([string]$HttpAttributeName)
    return ($HttpAttributeName -replace '^Http','').ToUpper()
}

function Join-UrlPath {
    param([string]$Base, [string]$Suffix)
    if ([string]::IsNullOrWhiteSpace($Suffix)) { return $Base }
    return ('{0}/{1}' -f $Base.TrimEnd('/'), $Suffix.TrimStart('/'))
}

function Convert-ToPostmanUrl {
    param(
        [string]$Url,
        [array]$QueryItems = @()
    )

    $urlObj = @{ raw = $Url }
    $pathPart = $Url
    $inlineQuery = @()

    if ($Url -match '^([^?]+)\?(.*)$') {
        $pathPart = $Matches[1]
        foreach ($pair in ($Matches[2] -split '&')) {
            if ($pair -match '^([^=]+)=(.*)$') {
                $inlineQuery += @{ key = $Matches[1]; value = $Matches[2]; description = "" }
            }
        }
    }

    if ($pathPart -match '^(https?)://(.+)$') {
        $urlObj.protocol = $Matches[1]
        $pathPart = $Matches[2]
    }

    $segments = @($pathPart.Trim('/').Split('/') | Where-Object { $_ })
    if ($segments.Count -gt 0) {
        $urlObj.host = @($segments[0])
        if ($segments.Count -gt 1) {
            $urlObj.path = @($segments[1..($segments.Count - 1)])
        }
    }

    $allQuery = @($inlineQuery) + @($QueryItems)
    if ($allQuery.Count -gt 0) {
        $urlObj.query = $allQuery
    }

    return $urlObj
}

function New-PostmanRequest {
    param(
        [string]$Name,
        [string]$Method,
        [string]$Url,
        [bool]$UseAuth,
        [string]$Description = "",
        [hashtable]$Query = @{},
        [string]$Body = $null,
        [hashtable]$Headers = @{}
    )

    $queryItems = @()
    foreach ($k in $Query.Keys) {
        $queryItems += @{ key = $k; value = [string]$Query[$k]; description = "" }
    }

    $headerItems = @(
        @{ key = "Accept"; value = "application/json"; type = "text" }
    )
    foreach ($k in $Headers.Keys) {
        $headerItems += @{ key = $k; value = [string]$Headers[$k]; type = "text" }
    }

    $request = @{
        method = $Method.ToUpper()
        header = $headerItems
        url = (Convert-ToPostmanUrl -Url $Url -QueryItems $queryItems)
        description = $Description
    }

    if ($UseAuth) {
        $request.auth = @{
            type = "bearer"
            bearer = @(@{ key = "token"; value = "{{accessToken}}"; type = "string" })
        }
    }
    else {
        $request.auth = @{ type = "noauth" }
    }

    if ($Body -ne $null -and $Method -in @('POST','PUT','PATCH')) {
        $request.header += @{ key = "Content-Type"; value = "application/json"; type = "text" }
        $request.body = @{ mode = "raw"; raw = $Body; options = @{ raw = @{ language = "json" } } }
    }

    return @{
        name = $Name
        request = $request
    }
}

function Get-ServiceBaseUrl {
    param([string]$ServiceKey, [string]$RouteTemplate, [hashtable]$GatewayMap)

    if ($GatewayMap.ContainsKey($RouteTemplate)) {
        return "{{gatewayUrl}}$($GatewayMap[$RouteTemplate])"
    }

    $varName = switch ($ServiceKey) {
        'UserService' { 'userServiceUrl' }
        'SetupService' { 'setupServiceUrl' }
        'NotificationService' { 'notificationServiceUrl' }
        'FileService' { 'fileServiceUrl' }
        'AuditService' { 'auditServiceUrl' }
        'PublisherService' { 'publisherServiceUrl' }
        'ConsumerService' { 'consumerServiceUrl' }
        'AssetService' { 'assetServiceUrl' }
        'BillingService' { 'billingServiceUrl' }
        'CommunicationService' { 'communicationServiceUrl' }
        'CrmService' { 'crmServiceUrl' }
        'IntegrationService' { 'integrationServiceUrl' }
        'InventoryService' { 'inventoryServiceUrl' }
        'ReportingService' { 'reportingServiceUrl' }
        'SchedulingService' { 'schedulingServiceUrl' }
        'ServiceAgreementService' { 'serviceAgreementServiceUrl' }
        default { 'gatewayUrl' }
    }

    # UserService tenants via gateway rewrite
    if ($ServiceKey -eq 'UserService' -and $RouteTemplate -eq 'tenants') {
        return "{{gatewayUrl}}/api/v1/users/tenants"
    }

    return "{{$varName}}/api/v1/$RouteTemplate"
}

function Parse-ControllerFile {
    param(
        [string]$FilePath,
        [string]$ServiceKey,
        [hashtable]$GatewayMap
    )

    $content = Get-Content -Raw -Path $FilePath
    $fileName = [IO.Path]::GetFileNameWithoutExtension($FilePath)
    if ($fileName -notmatch 'Controller$') { return $null }

    # Merge partial AuthController files
    if ($fileName -eq 'AuthController') {
        $dir = Split-Path $FilePath -Parent
        Get-ChildItem -Path $dir -Filter 'AuthController.*.cs' | ForEach-Object {
            $content += "`n" + (Get-Content -Raw -Path $_.FullName)
        }
    }

    $routeMatch = [regex]::Match($content, '\[FgsVersionedRoute\("([^"]+)"\)\]')
    if (-not $routeMatch.Success) { return $null }

    $routeTemplate = Get-RouteTemplate $routeMatch.Groups[1].Value $fileName
    $classHeader = ($content -split 'public\s+(?:sealed\s+)?(?:partial\s+)?class')[0]
    $baseUrl = Get-ServiceBaseUrl $ServiceKey $routeTemplate $GatewayMap

    $controllerDescription = "$fileName - $routeTemplate"

    $items = @()
    $methodRegex = [regex]::Matches($content, '(?s)((?:\s*///[^\r\n]*\r?\n)*)\s*(?:\[(?:AllowAnonymous|Authorize[^\]]*)\]\s*)*(\[Http(Get|Post|Put|Patch|Delete)(?:\("([^"]*)"\))?\])\s*(?:\[[^\]]+\]\s*)*public\s+(?:async\s+)?(?:Task<(?:IActionResult|ActionResult(?:<[^>]+>)?)>|ContentResult|IActionResult)\s+(\w+)\s*\(')

    foreach ($m in $methodRegex) {
        $block = $m.Value
        $httpAttr = $m.Groups[3].Value
        $routeSuffix = $m.Groups[4].Value
        $methodName = $m.Groups[5].Value
        $verb = Get-HttpVerb $httpAttr

        $docSummary = $null
        if ($m.Groups[1].Success -and $m.Groups[1].Value -match '///\s*<summary>\s*(.*?)\s*</summary>') {
            $docSummary = $Matches[1].Trim()
        }

        $useAuth = Test-RequiresAuth $classHeader $block
        $pathSuffix = Convert-RouteToPostmanPath $routeSuffix
        $fullPath = Join-UrlPath $baseUrl $pathSuffix

        $query = @{}
        $headers = @{}
        $body = $null

        if ($methodName -eq 'GetActive' -and $routeTemplate -eq 'communication-templates') {
            $query = @{ tenantId = '{{tenantId}}'; companyId = '{{companyId}}'; templateType = 'Email'; code = 'INVITE' }
            $headers['X-Internal-Service-Key'] = '{{internalServiceKey}}'
            $useAuth = $false
        }
        if ($methodName -eq 'Get' -and $fileName -eq 'DashboardController') {
            $fullPath = '{{gatewayUrl}}/api/v1/dashboard?token={{accessToken}}'
            $useAuth = $false
        }
        if ($methodName -eq 'List' -and $fileName -match 'Catalog|JobType|Inventory|Vendor|Warehouse|Vehicle|Lead|Sales|Setup|Billing|Business|Resolution|Tag') {
            $query = @{ page = '1'; pageSize = '25'; isActive = 'true' }
        }
        if ($httpAttr -in @('Post','Put','Patch') -and $methodName -notin @('EntraCallback','EntraConnector','CompanySignup','Start','ProvisionTenant','Record','Dispatch')) {
            $body = "{}"
        }
        if ($methodName -eq 'CompanySignup') {
            $body = @'
{
  "contact": { "name": "Admin User", "phoneNumber": "+15551234567", "email": "{{signupEmail}}" },
  "company": {
    "name": "Acme Field Services",
    "website": "https://acme.example.com",
    "companySize": "11-50",
    "address": { "addressLine1": "100 Main St", "city": "Austin", "state": "TX", "postalCode": "78701", "country": "US" }
  },
  "businessTypeIds": [1]
}
'@
        }
        if ($methodName -eq 'EntraConnector') {
            $body = '{ "email": "{{signupEmail}}", "objectId": null }'
        }
        if ($useAuth) {
            $headers['X-Tenant-Id'] = '{{tenantId}}'
            $headers['X-Company-Id'] = '{{companyId}}'
        }

        if ($methodName -eq 'Start') {
            $fullPath = '{{gatewayUrl}}/api/v1/invite/start?token={{inviteToken}}'
        }
        if ($methodName -eq 'EntraCallback') {
            $fullPath = '{{gatewayUrl}}/api/v1/auth/entra/callback?code={{authCode}}&state={{invitationId}}'
        }

        $displayName = $methodName
        if ($docSummary -and $docSummary.Length -le 48 -and $docSummary -notmatch '[.!?]') {
            $displayName = $docSummary
        }
        $description = if ($docSummary) { $docSummary } else { $methodName }
        $items += New-PostmanRequest -Name $displayName -Method $verb -Url $fullPath -UseAuth $useAuth -Description $description -Query $query -Body $body -Headers $headers
    }

    if ($items.Count -eq 0) { return $null }

    return @{
        name = $fileName
        description = $controllerDescription
        item = $items
    }
}

function New-AuthFlowFolder {
    $signupBody = @'
{
  "contact": { "name": "Admin User", "phoneNumber": "+15551234567", "email": "{{signupEmail}}" },
  "company": {
    "name": "Acme Field Services",
    "website": "https://acme.example.com",
    "companySize": "11-50",
    "address": { "addressLine1": "100 Main St", "city": "Austin", "state": "TX", "postalCode": "78701", "country": "US" }
  },
  "businessTypeIds": [1]
}
'@

    $signup = New-PostmanRequest -Name '1. Company Signup' -Method POST -Url '{{gatewayUrl}}/api/v1/signup/company' -UseAuth $false -Body $signupBody
    $signup | Add-Member -NotePropertyName event -NotePropertyValue @(@{
        listen = 'test'
        script = @{
            type = 'text/javascript'
            exec = @(
                'const body = pm.response.json();',
                'if (body.data) {',
                '  pm.environment.set("tenantId", String(body.data.tenantId));',
                '  pm.environment.set("companyId", String(body.data.companyNumber));',
                '  const m = body.data.inviteUrl.match(/[?&]token=([^&]+)/);',
                '  if (m) pm.environment.set("inviteToken", decodeURIComponent(m[1]));',
                '}'
            )
        }
    })

    $invite = New-PostmanRequest -Name '2. Start Invitation' -Method GET -Url '{{gatewayUrl}}/api/v1/invite/start?token={{inviteToken}}' -UseAuth $false
    $invite.request.description = 'Disable Follow Redirects. Copy Entra URL from Location header, open in browser, sign up, paste auth code into environment.'

    $token = @{
        name = '4. Entra Token Exchange'
        request = @{
            method = 'POST'
            auth = @{ type = 'noauth' }
            header = @(@{ key = 'Content-Type'; value = 'application/x-www-form-urlencoded'; type = 'text' })
            body = @{
                mode = 'urlencoded'
                urlencoded = @(
                    @{ key = 'grant_type'; value = 'authorization_code' }
                    @{ key = 'client_id'; value = '{{entraClientId}}' }
                    @{ key = 'client_secret'; value = '{{entraClientSecret}}' }
                    @{ key = 'code'; value = '{{authCode}}' }
                    @{ key = 'redirect_uri'; value = '{{redirectUri}}' }
                    @{ key = 'scope'; value = 'openid profile email offline_access' }
                )
            }
            url = (Convert-ToPostmanUrl -Url '{{entraAuthority}}/{{entraTenantId}}/oauth2/v2.0/token')
            description = 'Run after browser sign-up. Set authCode from callback URL.'
        }
        event = @(@{
            listen = 'test'
            script = @{
                type = 'text/javascript'
                exec = @(
                    'const body = pm.response.json();',
                    'if (body.access_token) pm.environment.set("accessToken", body.access_token);'
                )
            }
        })
    }

    return @{
        name = '00 - Authentication Flow'
        description = 'Run in order. Step 3 is manual: sign up in Entra with signupEmail, then set authCode.'
        item = @(
            $signup
            $invite
            @{ name = '3. Manual - Entra sign-up in browser'; request = @{ method = 'GET'; auth = @{ type = 'noauth' }; url = @{ raw = '(open Location URL from step 2)' }; description = 'Sign up with signupEmail. Copy code from callback into authCode.' } }
            $token
            (New-PostmanRequest -Name '5. Auth Me' -Method GET -Url '{{gatewayUrl}}/api/v1/auth/me' -UseAuth $true)
        )
    }
}

function New-Collection {
    param(
        [string]$ServiceName,
        [string]$Description,
        [array]$Folders,
        [array]$ExtraVariables = @(),
        [switch]$IncludeAuthFlow
    )

    $variables = @(
        @{ key = "apiVersion"; value = "v1" }
    ) + $ExtraVariables

    $items = @()
    if ($IncludeAuthFlow) {
        $items += New-AuthFlowFolder
    }
    $items += $Folders

    $collection = @{
        info = @{
            _postman_id = New-PostmanUuid
            name = "FGS $ServiceName"
            description = $Description
            schema = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
        }
        variable = $variables
        auth = @{
            type = "bearer"
            bearer = @(@{ key = "token"; value = "{{accessToken}}"; type = "string" })
        }
        event = @(
            @{
                listen = "prerequest"
                script = @{
                    type = "text/javascript"
                    exec = @(
                        "if (pm.environment.get('tenantId')) {",
                        "  pm.request.headers.upsert({ key: 'X-Tenant-Id', value: pm.environment.get('tenantId') });",
                        "  pm.request.headers.upsert({ key: 'X-Company-Id', value: pm.environment.get('companyId') });",
                        "}"
                    )
                }
            }
        )
        item = $items
    }

    return $collection
}

$gatewayMaps = @{
    UserService = @{
        'auth' = '/api/v1/auth'
        'invite' = '/api/v1/invite'
        'signup' = '/api/v1/signup'
        'dashboard' = '/api/v1/dashboard'
    }
    NotificationService = @{ 'notifications' = '/api/v1/notifications' }
    SetupService = @{
        'credentials' = '/api/v1/credentials'
        'communication-templates' = '/api/v1/communication-templates'
    }
    FileService = @{ 'tenants' = '/api/v1/tenants' }
}

$serviceConfigs = @(
    @{ Key = 'UserService'; Path = 'src\UserService\Fgs.User.API\Controllers'; Desc = 'Company onboarding, Entra auth, tenant admin APIs. Gateway-first; tenant routes use /api/v1/users/tenants.'; AuthFlow = $true }
    @{ Key = 'SetupService'; Path = 'src\SetupService\Fgs.Setup.API\Controllers'; Desc = 'Platform setup: credentials (gateway), catalog CRUD (direct setupServiceUrl), tenant provisioning.'; AuthFlow = $false }
    @{ Key = 'NotificationService'; Path = 'src\NotificationService\Fgs.Notification.API\Controllers'; Desc = 'Notification dispatch via gateway.'; AuthFlow = $false }
    @{ Key = 'FileService'; Path = 'src\FileService\Fgs.File.API\Controllers'; Desc = 'Tenant S3 bucket provisioning via gateway /api/v1/tenants.'; AuthFlow = $false }
    @{ Key = 'AuditService'; Path = 'src\AuditService\Fgs.Audit.API\Controllers'; Desc = 'Credential audit trail (direct service URL).'; AuthFlow = $false }
    @{ Key = 'PublisherService'; Path = 'src\PublisherService\Fgs.Publisher.API\Controllers'; Desc = 'Outbox publisher worker API.'; AuthFlow = $false }
    @{ Key = 'ConsumerService'; Path = 'src\ConsumerService\Fgs.Consumer.API\Controllers'; Desc = 'Message consumer worker API.'; AuthFlow = $false }
    @{ Key = 'AssetService'; Path = 'src\AssetService\Fgs.Asset.API\Controllers'; Desc = 'Asset service scaffold (health).'; AuthFlow = $false }
    @{ Key = 'BillingService'; Path = 'src\BillingService\Fgs.Billing.API\Controllers'; Desc = 'Billing service scaffold (health).'; AuthFlow = $false }
    @{ Key = 'CommunicationService'; Path = 'src\CommunicationService\Fgs.Communication.API\Controllers'; Desc = 'Communication service scaffold (health).'; AuthFlow = $false }
    @{ Key = 'CrmService'; Path = 'src\CrmService\Fgs.Crm.API\Controllers'; Desc = 'CRM service scaffold (health).'; AuthFlow = $false }
    @{ Key = 'IntegrationService'; Path = 'src\IntegrationService\Fgs.Integration.API\Controllers'; Desc = 'Integration service scaffold (health).'; AuthFlow = $false }
    @{ Key = 'InventoryService'; Path = 'src\InventoryService\Fgs.Inventory.API\Controllers'; Desc = 'Inventory service scaffold (health).'; AuthFlow = $false }
    @{ Key = 'ReportingService'; Path = 'src\ReportingService\Fgs.Reporting.API\Controllers'; Desc = 'Reporting service scaffold (health).'; AuthFlow = $false }
    @{ Key = 'SchedulingService'; Path = 'src\SchedulingService\Fgs.Scheduling.API\Controllers'; Desc = 'Scheduling service scaffold (health).'; AuthFlow = $false }
    @{ Key = 'ServiceAgreementService'; Path = 'src\ServiceAgreementService\Fgs.ServiceAgreement.API\Controllers'; Desc = 'Service agreement scaffold (health).'; AuthFlow = $false }
)

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

foreach ($svc in $serviceConfigs) {
    $controllerRoot = Join-Path $RepoRoot $svc.Path
    if (-not (Test-Path $controllerRoot)) { Write-Warning "Skip $($svc.Key): $controllerRoot"; continue }

    $map = if ($gatewayMaps.ContainsKey($svc.Key)) { $gatewayMaps[$svc.Key] } else { @{} }
    $files = Get-ChildItem -Path $controllerRoot -Filter '*Controller.cs' -Recurse |
        Where-Object { $_.Name -notmatch '^AuthController\..+\.cs$' } |
        Sort-Object FullName
    $folders = @()

    foreach ($file in $files) {
        $folder = Parse-ControllerFile -FilePath $file.FullName -ServiceKey $svc.Key -GatewayMap $map
        if ($null -ne $folder) { $folders += $folder }
    }

    if ($folders.Count -eq 0) { continue }

    # Group Setup generated controllers under sub-folder
    if ($svc.Key -eq 'SetupService') {
        $manual = $folders | Where-Object { $_.name -notmatch '^Setup|^Inventory|^Job|^Lead|^Sales|^Vendor|^Vehicle|^Warehouse|^Business|^Billing|^Resolution|^Tag' -or $_.name -in @('CredentialsController','CommunicationTemplatesController','TenantProvisioningController','BusinessTypesController','HealthController') }
        $catalog = $folders | Where-Object { $_.name -notin ($manual.name) }
        $grouped = @()
        if ($manual.Count -gt 0) {
            $grouped += @{ name = 'Platform & Admin'; item = $manual }
        }
        if ($catalog.Count -gt 0) {
            $grouped += @{ name = 'Catalog CRUD (Generated)'; description = 'Standard list/get/create/update/patch/delete. Requires Bearer token.'; item = $catalog }
        }
        $folders = $grouped
    }

    $includeAuth = ($svc.Key -eq 'UserService')
    $collection = New-Collection -ServiceName $svc.Key -Description $svc.Desc -Folders $folders -IncludeAuthFlow:$includeAuth
    $outFile = Join-Path $OutputDir "$($svc.Key).postman_collection.json"
    $collection | ConvertTo-Json -Depth 100 | Set-Content -Path $outFile -Encoding UTF8
    Write-Host "Generated $outFile ($($files.Count) controllers)"
}

Write-Host "Done. Import FGS-Globals.postman_environment.json and select it in Postman."
