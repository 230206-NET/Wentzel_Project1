using System.ComponentModel;
using System.Security.Cryptography;
using Models;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddScoped<DatabaseRepo>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "hello");

app.MapGet("/pending", ([FromServices] DatabaseRepo service) => service.getPendingExpenses());

app.MapGet("/expense", ([FromQuery] string? id, string? pass, [FromServices] DatabaseRepo service) =>  
pass.Equals(service.getPassByEmpId(id)) ? Results.Json(service.getExpensesByEmpId(int.Parse(id))) : Results.Unauthorized());


app.Run();
