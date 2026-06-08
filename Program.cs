using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. First, register core API/Controller infrastructure services
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer(); 

// 2. Second, register Swagger 
builder.Services.AddSwaggerGen(); 

var app = builder.Build(); // The service collection locks right here

// REMOVED DUPLICATE LINES FROM HERE (Lines 13 & 14 are gone)

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/AddUser", (string name,int age, string email) =>
{
    
    return Results.Ok(new { Name = name, Age = age, Email = email });
})
.WithName("AddUser");


app.MapPost("/GetUser", () =>
{
    
    return Results.Ok(new { Name = "Digvijay", Age = 25, Email = "digvijay@example.com" });
})
.WithName("GetUser");

app.UseAuthorization();
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
