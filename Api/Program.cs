var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var products = new List<Product>()
    {
        new Product ( 1, "HP Folio", 150, 5),
        new Product ( 2, "Dell XPS 17", 350,  8),
        new Product ( 3, "Macbook Pro", 1430,  3),
        new Product ( 4, "HP Omen", 980,  7)
    };

app.MapGet("/data", () =>
{
    return products;
})
.WithName("GetProducts");

app.MapGet("/data/{id}", (int id) =>
{
    return products.Find(product => product.Id == id);
})
.WithName("GetProductById");

app.Run();

internal record Product(int Id, string Name, double Price, int Quantity);
