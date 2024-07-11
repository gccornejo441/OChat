$projectFile = Get-ChildItem -Path . -Filter *.csproj -Recurse | Select-Object -First 1


if ($projectFile -ne $null) {
    $projectFilePath = $projectFile.FullName
    # Proceed with using the $projectFilePath
    Write-Host "Project file found: $projectFilePath"
} else {
    Write-Host "No project file found."
}

[xml]$xml = Get-Content $projectFilePath
$versionNode = $xml.Project.PropertyGroup.Version
$versionParts = $versionNode.'#text' -split '\.'
$versionParts[2] = [int]$versionParts[2] + 1 # Increment patch version
$newVersion = $versionParts -join '.'
$versionNode.'#text' = $newVersion
$xml.Save($projectFile)
