Write-Host "Start Docker" -ForegroundColor green
Start-Process PowerShell -WorkingDirectory "." -ArgumentList "-noexit -command docker compose up"

Write-Host "Start BirdsApi" -ForegroundColor green
Start-Process PowerShell -WorkingDirectory ".\BirdsApi" -ArgumentList "-noexit -command dotnet watch run --launch-profile https"

Write-Host "Start BirdsApp" -ForegroundColor green
Start-Process PowerShell -WorkingDirectory ".\BirdsApp" -ArgumentList "-noexit -command ng serve --open"