# How to run the application?

## Easy way via scripts

Clone this repository and open a Powershell terminal in the root folder of this repository.

To install the pre-requisites (for Windows 10 1809 build 17763 or newer only):
`.\install.ps1`

To build the application:
`.\build.ps1`

To run the application:
`.\run.ps1`


## Manually start the application (debugging)

### Frontend

From the root of the repository, execute:

1. `cd BirdsApp`
2. `ng serve --open`

### Backend

From the root of the repository, execute:

1. `cd BirdsApi`
2. `dotnet dev-certs https --trust`
3. `dotnet watch run --launch-profile https`

## Docker and DevContainers

From the root of the repository, execute:

1. `docker compose up`


# How to create the application from scratch?

## Angular

Install Node.js: `winget install --id OpenJS.NodeJS.LTS --exact`
Install the Angular CLI https://angular.io/guide/setup-local#install-the-angular-cli

From the root of the repository, execute:

```
ng new BirdsApp --directory BirdsApp
```

## DotNet

Install the .NET SDK: `winget install --id Microsoft.DotNet.SDK.7 --exact`

From the root of the repository, execute:

```
md BirdsApi
cd BirdsApi
dotnet new gitignore
dotnet new sln
dotnet new webapi --auth none --use-minimal-apis --use-program-main false
dotnet sln add BirdsApi
dotnet dev-certs https --trust
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite
dotnet add package NetTopologySuite.IO.GeoJSON4STJ
dotnet add package FluentValidation
```

## Docker and DevContainers

Install Docker Desktop: `winget install --id Docker.DockerDesktop --exact`