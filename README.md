<p align="center"><img src="https://github.com/PostOShare/IdentityWebApi/assets/17848426/b2a55da5-dc7f-4b33-9de6-e6d0c641a752" alt="logo"></p>

<div align="center">
  
[![GitHub Issues](https://img.shields.io/github/issues/PostOShare/PostWebApi.svg)](https://github.com/PostOShare/PostWebApi/issues)
[![GitHub license](https://img.shields.io/github/license/PostOShare/PostWebApi)](https://github.com/PostOShare/PostWebApi/blob/master/LICENSE)
![Static Badge](https://img.shields.io/badge/.net-9-green)

</div>

PostWebApi is a .NET 9 Web API used to manage post related data in PostOShare. It depends on [.NET 9](https://dotnet.microsoft.com/en-us/) and communicates with a [MongoDB](https://www.mongodb.com/) database running in [Docker](https://www.docker.com/).

# Prerequisites

The following need to be available to ensure that the API and the Mongo DB database can be setup:
- [.NET 9](https://dotnet.microsoft.com/en-us/)
- [Visual Studio 2022 or higher](https://visualstudio.microsoft.com/downloads/)

Additionally, you may use the following to create containers, execute queries and send API endpoint request:
- [Docker Desktop](https://docs.docker.com/desktop/)
- [MongoDB Compass](https://www.mongodb.com/products/tools/compass)
- [Postman](https://www.postman.com/)

Additionally, [IdentityWebApi](https://github.com/PostOShare/IdentityWebApi/tree/main) needs to be running in order to validate access tokens. This requires that the following is available:
- [.NET 6](https://dotnet.microsoft.com/en-us/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2022)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

# Installation

Steps that can be used to setup the API are

```cmd
git clone https://github.com/PostOShare/PostWebApi.git

cd PostWebApi\PostWebApi
dotnet restore
```

Then open a ```cmd``` and run the following to create a MongoDB Docker container (replace the username and password with your intended username and password):

```cmd
docker run -d ^
  --name mongodb ^
  -p 27017:27017 ^
  -v mongo_data:/data/db ^
  -e MONGO_INITDB_ROOT_USERNAME=admin ^
  -e MONGO_INITDB_ROOT_PASSWORD=your_secure_password ^
  mongodb/mongodb-community-server:latest
```

or if you use ```bash``` run the following: 

```bash
docker run -d \
  --name mongodb \
  -p 27017:27017 \
  -v mongo_data:/data/db \
  -e MONGO_INITDB_ROOT_USERNAME=admin \
  -e MONGO_INITDB_ROOT_PASSWORD=your_secure_password \
  mongodb/mongodb-community-server:latest
```

After installation, you can start and stop this container with the Docker Desktop.

Open MongoDB Compass, connect to the server and run the [script](https://github.com/PostOShare/PostWebAPI/blob/v1/Database%20Scripts/database_creation.js) in a shell to create the database.

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

# Endpoints of the API

## api/v1/Post/create-post

This endpoint is used to save a post to the database. This endpoint needs a valid access token in the header in the form of Bearer Token. Please generate one based on the username from the [generate-accesstoken](https://github.com/PostOShare/IdentityWebApi#apiv1authgenerate-accesstoken) endpoint.

### Sample request

```
curl -X 'POST' \
  'https://localhost:7005/api/v1/Post/create-post' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJ1c2VyNCIsIm5iZiI6MTc5MTAzOTc2NywiZXhwIjoxNzkxMTI2MTY3LCJpYXQiOjE3OTEwMzk3NjcsImlzcyI6Imh0dHBzOi8vd3d3LnBvc3Rvc2hhcmUuY29tLyJ9.mIOdSBXYeNVsoSy6erHvmHW3GlQEoKWa0rvmytcHgBI" \
  -d '{
    "partitionKey": "user4",
    "type": "post",
    "content": "Look at this amazing sunset!",
    "media": [
        {
            "type": "image",
            "url": "https://cdn.myapp.com/blobs/sunset.jpg",
            "createdAt": "2026/08/31 21:30:00",
            "visibility": "public"
        }
    ],
    "createdAt": "2026/08/31 21:30:00",
    "lastUpdatedAt": "2026/08/31 21:30:00",
    "agent": "web",
    "visibility": "public",
    "stats": {
        "thumbsUpReactions": 0,
        "hahaReactions": 0,
        "angryReactions": 0,
        "loveReactions": 0,
        "comments": 0,
        "share": 0
    },
    "comments": [
        {
            "commentId": "c_001",
            "userId": "user_456",
            "text": "Sample text",
            "createdAt": "2026/08/31 21:33:00",
            "lastUpdatedAt": "2026/08/31 21:33:00",
            "subComments": [
                {
                    "subCommentId": "sc_001",
                    "userId": "user_123",
                    "text": "Sample text",
                    "createdAt": "2026/08/31 21:35:00",
                    "lastUpdatedAt": "2026/08/31 21:30:00"
                }
            ]
        }
    ]
}'
```

### Responses

- 201 - Post inserted
  	
  ```json
  {    
    "result": true,
    "errorDescription": "",
    "errorCode": "00000"
  }
  ```

- 400 -

  ```json
  {
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "PartitionKey": [
            "partitionKey is required for Cosmos DB compatibility"
        ]
    },
    "traceId": "00-c603c299c831b1043fe68ec409cb9d20-4e13003f77fb5e07-00"
  }
  ```

- 500 -
  
  ```json
  {
    "result": false,
    "errorCode": "",
    "errorDesc": "No connection could be made because the target machine actively refused it. (localhost:7224)",
    "posts": null
  }
  ```

## api/v1/Post/update-post

This endpoint is used to update a post of the database. This endpoint needs a valid access token in the header in the form of Bearer Token. Please generate one based on the username from the [generate-accesstoken](https://github.com/PostOShare/IdentityWebApi#apiv1authgenerate-accesstoken) endpoint.

### Sample request


```
curl -X 'PUT' \
  'https://localhost:7005/api/v1/Post/6ab7e2eeaf52567676d23196' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJ1c2VyNCIsIm5iZiI6MTc5MTAzOTc2NywiZXhwIjoxNzkxMTI2MTY3LCJpYXQiOjE3OTEwMzk3NjcsImlzcyI6Imh0dHBzOi8vd3d3LnBvc3Rvc2hhcmUuY29tLyJ9.mIOdSBXYeNVsoSy6erHvmHW3GlQEoKWa0rvmytcHgBI" \
  -d '{
    "partitionKey": "user_123",
    "type": "post",
    "content": "Look at this amazing sunset!",
    "media": [
        {
            "type": "image",
            "url": "https://cdn.myapp.com/blobs/sunset1.jpg",
            "createdAt": "2026/08/31 21:30:00",
            "visibility": "public"
        }
    ],
    "createdAt": "2026/08/31 21:30:00",
    "lastUpdatedAt": "2026/08/31 21:30:00",
    "agent": "web",
    "visibility": "public",
    "stats": {
        "thumbsUpReactions": 14,
        "hahaReactions": 0,
        "angryReactions": 0,
        "loveReactions": 8,
        "comments": 0,
        "share": 0
    },
    "comments": [
        {
            "commentId": "c_001",
            "userId": "user_456",
            "text": "Sample text",
            "createdAt": "2026/08/31 21:33:00",
            "lastUpdatedAt": "2026/08/31 21:33:00",
            "subComments": [
                {
                    "subCommentId": "sc_001",
                    "userId": "user_123",
                    "text": "Sample text",
                    "createdAt": "2026/08/31 21:35:00",
                    "lastUpdatedAt": "2026/08/31 21:30:00"
                }
            ]
        },
        {
            "commentId": "c_002",
            "userId": "user_458",
            "text": "Sample text1",
            "createdAt": "2026/09/24 21:33:00",
            "lastUpdatedAt": "2026/09/24 21:33:00",
            "subComments": []
        }
    ]
}'
```


### Responses

- 200 - Post update completed
  	
  ```json
  {    
    "result": true,
    "errorDescription": "",
    "errorCode": "00000"
  }
  ```

- 400 -

  ```json
  {
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "PartitionKey": [
            "partitionKey is required for Cosmos DB compatibility"
        ]
    },
    "traceId": "00-c603c299c831b1043fe68ec409cb9d20-4e13003f77fb5e07-00"
  }
  ```

  - 401 -

  ```json
  {
    "result": false,
    "errorCode": "00003",
    "errorDesc": "User is not authorized to delete this post.",
    "posts": null
  }
  ```

- 500 -
  
  ```json
  {
    "result": false,
    "errorCode": "",
    "errorDesc": "No connection could be made because the target machine actively refused it. (localhost:7224)",
    "posts": null
  }
  ```
