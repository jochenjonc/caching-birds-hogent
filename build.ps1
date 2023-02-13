Write-Host "Build BirdsApi" -ForegroundColor green
cd BirdsApi
dotnet build
dotnet dev-certs https --trust
cd ..

Write-Host "Build BirdsApp" -ForegroundColor green
cd BirdsApp
npm install
ng build
cd ..