# This script automates the process of incrementing the version number in both the project file
# and the appsettings.json file for a given project. 

param(
    [string]$ProjectName,
    [string]$AppSettingsFileName = "appsettings.json"
)
$directory = "."

$projectFilePath = Get-ChildItem -Path $directory -Filter "$ProjectName.csproj" -Recurse | Select-Object -First 1
$appSettingsFilePath = Get-ChildItem -Path $directory -Filter $AppSettingsFileName -Recurse | Select-Object -First 1

if ($projectFilePath -eq $null) {
    Write-Host "Project file not found."
    exit 1
} else {
    Write-Host "Project file found at $($projectFilePath.FullName)"
}

if ($appSettingsFilePath -eq $null) {
    Write-Host "Appsetting file not found."
    exit 1
} else {
    Write-Host "Appsetting file found at $($appSettingsFilePath.FullName)"
}

[xml]$projectFile = Get-Content $projectFilePath.FullName
$propertyGroup = $projectFile.Project.PropertyGroup | Where-Object { $_.FileVersion }
$currentVersion = $propertyGroup.FileVersion
$versionParts = $currentVersion -split '\.'

if ($versionParts.Length -ne 3) {
    Write-Host "Invalid version number format in .csproj file. Expected format: Major.Minor.Patch"
    exit 1
}

$versionParts[2] = [int]$versionParts[2] + 1
$newVersion = $versionParts -join '.'

$propertyGroup.FileVersion = $newVersion
$projectFile.Save($projectFilePath.FullName)
Write-Host "Updated .csproj file to version $newVersion"

$appSettings = Get-Content $appSettingsFilePath.FullName | ConvertFrom-Json
$appSettings.OllamaClientAppSettings.Version = $newVersion
$appSettings | ConvertTo-Json -Depth 32 | Set-Content $appSettingsFilePath.FullName
Write-Host "Updated appsettings.json file to version $newVersion"