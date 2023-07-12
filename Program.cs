using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// The call to AddEndpointsApiExplorer shown is required only for minimal APIs.
builder.Services.AddEndpointsApiExplorer();

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Book API",
        Description = "An ASP.NET Core Web API for managing books",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Kenan Hancer",
            Email = "kenanhancer@gmail.com",
            Url = new Uri("https://kenanhancer.com")
        },
        License = new OpenApiLicense
        {
            Name = "Book License",
            Url = new Uri("https://example.com/license")
        }
    });
});

builder.Services.AddSingleton<IDateTime, SystemDateTime>();

builder.Services.AddSingleton<List<Book>>((serviceProvider) =>
    new List<Book>()
    {
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000001"), Title = "Introduction to Algorithms", Author = "Thomas H. Cormen" },
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000002"), Title = "Design Patterns: Elements of Reusable Object-Oriented Software", Author = "Erich Gamma" },
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000003"), Title = "Clean Code: A Handbook of Agile Software Craftsmanship", Author = "Robert C. Martin" },
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000004"), Title = "You Don't Know JS: ES6 & Beyond", Author = "Kyle Simpson" },
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000005"), Title = "The Pragmatic Programmer", Author = "Andrew Hunt" },
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000006"), Title = "Code: The Hidden Language of Computer Hardware and Software", Author = "Charles Petzold" },
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000007"), Title = "Structure and Interpretation of Computer Programs", Author = "Harold Abelson" },
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000008"), Title = "Refactoring: Improving the Design of Existing Code", Author = "Martin Fowler" },
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000009"), Title = "Cracking the Coding Interview: 189 Programming Questions and Solutions", Author = "Gayle Laakmann McDowell" },
        new Book { Id = new Guid("00000000-0000-0000-0000-000000000010"), Title = "Grokking Algorithms: An illustrated guide for programmers and other curious people", Author = "Aditya Bhargava" }
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(options =>
    {
        // To serve the Swagger UI at the app's root (https://localhost:<port>/), set the RoutePrefix property to an empty string:
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

var _logger = app.Services.GetService<ILogger<Program>>();

app.MapGet(pattern: "/books", ([FromServices] List<Book> bookList, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation("books endpoint is called.");

    return TypedResults.Ok(bookList);
})
.WithName("GetBooks")
.WithDescription("Fetches all books")
.WithGroupName("v1")
.WithSummary("Fetches all books")
.WithTags("Book Get Operations")
.WithOpenApi();

app.MapGet("/books/id/{id}", Results<Ok<Book>, NotFound> ([FromRoute] Guid id, [FromServices] List<Book> bookList, [FromServices] ILogger<Program> logger) =>
{
    _logger?.LogInformation($"book by id endpoint is called. id is {id}");

    logger.LogInformation("book by id endpoint is called.");

    return bookList.FirstOrDefault(book => book.Id == id) is Book book
        ? TypedResults.Ok(book)
        : TypedResults.NotFound();
})
.WithName("GetBooksById")
.WithDescription("Fetches books by id")
.WithGroupName("v1")
.WithSummary("Fetches books by id")
.WithTags("Book Get Operations")
.WithOpenApi();

app.MapGet("/books/author/{author}", Results<Ok<List<Book>>, NotFound> ([FromRoute] string author,
    [FromServices] List<Book> bookList, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation($"Get book by author endpoint is called. author is {author}");

    return bookList.Where(book => book.Author == author).ToList() is List<Book> booksByAuthor
        ? TypedResults.Ok(booksByAuthor)
        : TypedResults.NotFound();
})
.WithName("GetBooksByAuthor")
.WithDescription("Fetches books by author")
.WithGroupName("v1")
.WithSummary("Fetches books by author")
.WithTags("Book Get Operations")
.WithOpenApi();

app.MapGet("/books/title/{title}", Results<Ok<List<Book>>, NotFound> ([FromRoute] string title, [FromServices] List<Book> bookList, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation($"Get book by title endpoint is called. title is {title}");

    // null coalescing ?? operator is used
    return bookList.Where(book => book.Title?.Contains(title, StringComparison.OrdinalIgnoreCase) ?? false).ToList() is
        List<Book> booksByTitle
        ? TypedResults.Ok(booksByTitle)
        : TypedResults.NotFound();
})
.WithName("GetBooksByTitle")
.WithDescription("Fetches books by title")
.WithGroupName("v1")
.WithSummary("Fetches books by title")
.WithTags("Book Get Operations")
.WithOpenApi();

app.MapPost("/book", Results<Created<Book>, BadRequest> ([FromBody] Book newBook, [FromServices] List<Book> bookList, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation("New book submission endpoint is called.");

    if (bookList.Any(book => book.Title == newBook.Title))
    {
        return TypedResults.BadRequest();
    }

    newBook.Id = Guid.NewGuid();
    bookList.Add(newBook);
    
    return TypedResults.Created($"/book/{newBook.Id}", newBook);
})
.WithName("AddNewBook")
.WithDescription("Add new book")
.WithGroupName("v1")
.WithSummary("Add new book")
.WithTags("Book Set Operations")
.WithOpenApi();

app.MapPut("/book/{id}", Results<Ok<Book>, NotFound, BadRequest> ([FromRoute] Guid id, [FromBody] Book updatedBook, [FromServices] List<Book> bookList, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation("Book update endpoint is called.");

    var existingBook = bookList.FirstOrDefault(book => book.Id == id);

    if (existingBook is null)
    {
        return TypedResults.NotFound();
    }

    if (updatedBook.Id != id)
    {
        return TypedResults.BadRequest();
    }

    existingBook.Title = updatedBook.Title;
    existingBook.Author = updatedBook.Author;

    return TypedResults.Ok(existingBook);
})
.WithName("ReplaceExistingBook")
.WithDescription("Replace existing book")
.WithGroupName("v1")
.WithSummary("Replace existing book")
.WithTags("Book Set Operations")
.WithOpenApi();

app.MapDelete("/book/{id}", Results<Ok, NotFound> ([FromRoute] Guid id, [FromServices] List<Book> bookList, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation("Book deletion endpoint is called.");

    var existingBook = bookList.FirstOrDefault(book => book.Id == id);

    if (existingBook is null)
    {
        return TypedResults.NotFound();
    }

    bookList.Remove(existingBook);

    return TypedResults.Ok();
})
.WithName("DeleteBook")
.WithDescription("Delete book")
.WithGroupName("v1")
.WithSummary("Delete book")
.WithTags("Book Set Operations")
.WithOpenApi();

app.Run();

internal class Book
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
}

internal interface IDateTime
{
    DateTime Now { get; }
}

internal class SystemDateTime : IDateTime
{
    public DateTime Now => throw new NotImplementedException();
}