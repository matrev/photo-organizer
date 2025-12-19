# Cross-Platform Build Script for Photo Organizer
# Run this script from the project root directory

param(
    [Parameter(Mandatory=$false)]
    [string]$TargetRid,
    [Parameter(Mandatory=$false)]
    [switch]$AllPlatforms,
    [Parameter(Mandatory=$false)]
    [string]$OutputDir = "./publish"
)

$ErrorActionPreference = "Stop"

# Define supported runtime identifiers
$rids = @(
    "win-x64",      # Windows 64-bit
    "win-x86",      # Windows 32-bit
    "win-arm64",    # Windows ARM64
    "osx-x64",      # macOS Intel
    "osx-arm64",    # macOS Apple Silicon
    "linux-x64",    # Linux 64-bit
    "linux-arm64",   # Linux ARM64
    "linux-musl-x64", # Linux musl 64-bit (Alpine)
    "linux-musl-arm64" # Linux musl ARM64
)

function Build-ForRid {
    param([string]$rid)

    Write-Host "Building for $rid..." -ForegroundColor Green

    $outputPath = Join-Path $OutputDir $rid

    # Create output directory if it doesn't exist
    if (!(Test-Path $outputPath)) {
        New-Item -ItemType Directory -Path $outputPath | Out-Null
    }

    # Build and publish
    & dotnet publish -c Release -r $rid --self-contained --output $outputPath

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Successfully built for $rid" -ForegroundColor Green
        $exeName = if ($rid.StartsWith("win")) { "photo-organizer.exe" } else { "photo-organizer" }
        $exePath = Join-Path $outputPath $exeName
        if (Test-Path $exePath) {
            $size = (Get-Item $exePath).Length / 1MB
            Write-Host "Output: $exePath ($('{0:N2}' -f $size) MB)" -ForegroundColor Cyan
        }
    } else {
        Write-Error "Failed to build for $rid"
        exit 1
    }
}

# Main logic
if ($AllPlatforms) {
    Write-Host "Building for all supported platforms..." -ForegroundColor Yellow
    foreach ($rid in $rids) {
        Build-ForRid -rid $rid
    }
    Write-Host "All builds completed!" -ForegroundColor Green
} elseif ($TargetRid) {
    if ($rids -contains $TargetRid) {
        Build-ForRid -rid $TargetRid
    } else {
        Write-Error "Unsupported RID: $TargetRid. Supported RIDs: $($rids -join ', ')"
        exit 1
    }
} else {
    Write-Host "Usage:" -ForegroundColor Yellow
    Write-Host "  .\build.ps1 -AllPlatforms              # Build for all platforms" -ForegroundColor White
    Write-Host "  .\build.ps1 -TargetRid win-x64          # Build for specific platform" -ForegroundColor White
    Write-Host "  .\build.ps1 -TargetRid linux-x64 -OutputDir ./my-output  # Custom output directory" -ForegroundColor White
    Write-Host ""
    Write-Host "Supported platforms:" -ForegroundColor Yellow
    $rids | ForEach-Object { Write-Host "  - $_" -ForegroundColor White }
}