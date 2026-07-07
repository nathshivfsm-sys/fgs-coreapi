$ErrorActionPreference = 'Stop'
$infra = 'c:\SourceCode\FGS\src\SetupService\Fgs.Setup.Infrastructure'
$entitiesRoot = Join-Path $infra 'Entities'

$entityFolders = @(
    'BillingCategories', 'CommunicationTemplates', 'FgsBusinessTypes', 'GLBreaks',
    'JobTypeCategories', 'JobTypes', 'JobTypeSubCategories',
    'LeadDisqualificationReasons', 'LeadSources', 'LeadStatuses',
    'ResolutionCodes', 'SalesActivityOutcomes', 'SalesActivityTypes',
    'SalesDispositionReasons', 'SalesPipelineStatuses',
    'SetupDescriptions', 'SetupLaborRateTypes', 'SetupPaymentMethods',
    'SetupPaymentTerms', 'SetupPostalCodes', 'SetupTaxAuthorities',
    'SetupTaxes', 'SetupTechSkillLevels', 'SetupTimeSlots', 'SetupZones',
    'Tags', 'TechTrades', 'TitlesOfCourtesy', 'VehicleMaintenances', 'Vehicles',
    'UniversalPricingMatrix'
)

New-Item -ItemType Directory -Path $entitiesRoot -Force | Out-Null

foreach ($folder in $entityFolders) {
    $source = Join-Path $infra $folder
    $dest = Join-Path $entitiesRoot $folder
    if (Test-Path $source) {
        if (Test-Path $dest) {
            Write-Host "Skip move (already exists): $folder"
        } else {
            Move-Item -Path $source -Destination $dest
            Write-Host "Moved: $folder"
        }
    } elseif (Test-Path $dest) {
        Write-Host "Already under Entities: $folder"
    } else {
        Write-Warning "Missing: $folder"
    }
}

$setupService = 'c:\SourceCode\FGS\src\SetupService'
$csFiles = Get-ChildItem -Path $setupService -Recurse -Filter '*.cs' -File

# Universal nested paths first
$universalChildren = @(
    'UniversalPricingServices', 'UniversalMatrixTiers', 'UniversalMatrixSizeTiers',
    'UniversalMatrixItems', 'UniversalMatrixFrequencyDiscounts',
    'UniversalMatrixOneTimeFees', 'UniversalMatrixAddOns'
)
foreach ($child in $universalChildren) {
    $old = "Fgs.Setup.Infrastructure.UniversalPricingMatrix.$child"
    $new = "Fgs.Setup.Infrastructure.Entities.UniversalPricingMatrix.$child"
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName -Raw
        if ($content -match [regex]::Escape($old)) {
            Set-Content -Path $file.FullName -Value ($content.Replace($old, $new)) -NoNewline
        }
    }
}

# Fix stale universal usings without UniversalPricingMatrix prefix
foreach ($child in $universalChildren) {
    $old = "Fgs.Setup.Infrastructure.$child"
    $new = "Fgs.Setup.Infrastructure.Entities.UniversalPricingMatrix.$child"
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName -Raw
        if ($content -match [regex]::Escape($old)) {
            Set-Content -Path $file.FullName -Value ($content.Replace($old, $new)) -NoNewline
        }
    }
}

# Top-level entity folders (exclude UniversalPricingMatrix - handled above)
foreach ($folder in $entityFolders) {
    if ($folder -eq 'UniversalPricingMatrix') { continue }
    $old = "Fgs.Setup.Infrastructure.$folder"
    $new = "Fgs.Setup.Infrastructure.Entities.$folder"
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName -Raw
        if ($content -match [regex]::Escape($old)) {
            Set-Content -Path $file.FullName -Value ($content.Replace($old, $new)) -NoNewline
        }
    }
}

Write-Host 'Done reorganizing Infrastructure entity folders.'
