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

## Fetches all books
```shell
curl https://localhost:7002/books
```

## Fetches books by id
```shell
curl https://localhost:7002/books/id/1
```

## Fetches books by author
```shell
curl https://localhost:7002/books/author/Meredith Alonso
```

## Fetches books by title
```shell
curl https://localhost:7002/books/title/Learn Linq
```

## Create book
```shell
curl -X POST -H "Content-Type: application/json" -d '{"title":"The Art of Computer Programming", "author":"Donald E. Knuth"}' https://localhost:7002/book
```

## Update Book
```shell
curl -X PUT -H "Content-Type: application/json" -d '{"id":"00000000-0000-0000-0000-000000000003","title":"Operating System Concepts","author":"Abraham Silberschatz"}' https://localhost:7002/book/00000000-0000-0000-0000-000000000003
```

## Delete Book
```shell
curl -X DELETE https://localhost:7002/book/00000000-0000-0000-0000-000000000001
```

## Swagger UI Endpoints
```
http://localhost:5261/swagger
```

```
https://localhost:7002/swagger
```

![](/assets/image1.png)