Write-Host "Install .NET SDK 7" -ForegroundColor green
winget install --id Microsoft.DotNet.SDK.7 --exact

Write-Host "Install Node.js" -ForegroundColor green
winget install --id OpenJS.NodeJS.LTS --exact

Write-Host "Install Docker Desktop" -ForegroundColor green
winget install --id Docker.DockerDesktop --exact

Write-Host "Install Microsoft SQL Server Management Studio" -ForegroundColor green
winget install -e --id Microsoft.SQLServerManagementStudio --exact