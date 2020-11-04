[CmdletBinding()]
param(
    [switch]$Build
)

$projectRoot = "$PSScriptRoot/.."
Push-Location $projectRoot

if($Build) {
    & docker-compose build
}
& docker-compose up --remove-orphans

Pop-Location
