# aspnet-core-book-minimal-web-api
ASP.NET Core Book Minimal Web API

![](/assets/image1.png)

Find details in [https://kenanhancer.com/2023/07/10/how-to-create-asp-net-core-minimal-web-api/?preview=true](https://kenanhancer.com/2023/07/10/how-to-create-asp-net-core-minimal-web-api/?preview=true)

## Build Project
```shell
dotnet build
```

## Run Project
```shell
dotnet run
```

> Update port number in curl requests after running project. You can find it in Debug Console

## All Books
```shell
curl https://localhost:7002/books
```

## Get Book by ID
```shell
curl https://localhost:7002/book/1
```

## Swagger UI Endpoints
```
http://localhost:5261/swagger
```

```
https://localhost:7002/swagger
```

![](/assets/image1.png)