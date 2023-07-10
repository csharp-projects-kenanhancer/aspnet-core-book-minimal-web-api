using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<List<Book>>((serviceProvider) =>
    new()
    {
        new Book(1, "Testing Dot", "Carson Alexander"),
        new Book(2, "Learn Linq", "Meredith Alonso"),
        new Book(3, "Generics", "Arturo Anand"),
        new Book(4, "Testing the Mic", "Gytis Barzdukas"),
        new(5, "Drop the Dot", "Van Li")
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/books", (List<Book> bookList) => TypedResults.Ok(bookList));

app.MapGet("/book/{id}", Results<Ok<Book>, NotFound> (int id, List<Book> bookList) =>
{
    return bookList.FirstOrDefault(book => book.Id == id) is Book book
        ? TypedResults.Ok(book)
        : TypedResults.NotFound();
});

app.Run();

record Book(int Id, string Title, string Author);