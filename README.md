<p align="center"><img src="https://github.com/PostOShare/IdentityWebApi/assets/17848426/b2a55da5-dc7f-4b33-9de6-e6d0c641a752" alt="logo"></p>

<div align="center">
  
[![GitHub Issues](https://img.shields.io/github/issues/PostOShare/PostWebApi.svg)](https://github.com/PostOShare/PostWebApi/issues)
[![GitHub license](https://img.shields.io/github/license/PostOShare/PostWebApi)](https://github.com/PostOShare/PostWebApi/blob/master/LICENSE)
![Static Badge](https://img.shields.io/badge/.net-9-green)

</div>

PostWebApi is a .NET 9 Web API used to manage post related data in PostOShare. It depends on [.NET 9](https://dotnet.microsoft.com/en-us/) and communicates with a [MongoDB](https://www.mongodb.com/) database.

# Prerequisites

The following need to be available to ensure that the API and the Mongo DB database can be setup:

- [.NET 9](https://dotnet.microsoft.com/en-us/)
- [Visual Studio 2022 or higher](https://visualstudio.microsoft.com/downloads/)
- [Docker Desktop](https://docs.docker.com/desktop/)
- [MongoDB Compass](https://www.mongodb.com/products/tools/compass)
- [Postman](https://www.postman.com/)

Additionally, it requires that [IdentityWebApi](https://github.com/PostOShare/IdentityWebApi/tree/main) is running. This requires that the following is available:

- [.NET 6](https://dotnet.microsoft.com/en-us/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2022)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

# Installation

# Installation

Steps that can be used to setup the API are

```cmd
git clone https://github.com/PostOShare/PostWebApi.git

cd PostWebApi\PostWebApi
dotnet restore
```

Before starting the API install the [IdentityWebApi](https://github.com/PostOShare/IdentityWebApi/blob/main/README.md#installation) and execute this API. Then execute the PostWebApi.

# Dependencies

## Production dependencies

-  Microsoft.AspNetCore.Authorization
-  Microsoft.AspNetCore.Authentication.JwtBearer
-  Serilog.AspNetCore
-  Serilog.Sinks.File
-  Microsoft.Extensions.Http
-  MongoDB.Bson
-  Microsoft.AspNetCore.Http
-  Microsoft.Extensions.Configuration.Abstractions
-  MongoDB.Driver

## Development dependencies

These dependencies are used only in development:

- Swashbuckle.AspNetCore
- Swashbuckle.AspNetCore.Annotations
- Microsoft.AspNetCore.OpenApi
