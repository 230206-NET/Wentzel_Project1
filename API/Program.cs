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

app.MapGet("/pending", ([FromQuery] string? id, string? pass, [FromServices] DatabaseRepo service) => 
pass.Equals(service.getPassByEmpId(id)) ? Results.Json(service.getPendingExpenses()) : Results.Unauthorized());


app.MapGet("/expense", ([FromQuery] string? id, string? pass, [FromServices] DatabaseRepo service) =>  
pass.Equals(service.getPassByEmpId(id)) ? Results.Json(service.getExpensesByEmpId(int.Parse(id))) : Results.Unauthorized());

app.MapPost("/expense", ([FromQuery] string note, string pass, string empid, decimal value, [FromServices] DatabaseRepo service) =>
pass.Equals(service.getPassByEmpId(empid)) ? Results.Created("/expense", service.putNewExpense(note,int.Parse(empid),value)) : Results.Unauthorized());

app.MapPost("/register", ([FromQuery] string name, string pass, [FromServices] DatabaseRepo service) => {
    return Results.Json(service.newEmployee(name,pass));
});

app.MapPost("/process", ([FromQuery] string status, int expid, string empid, string pass, [FromServices] DatabaseRepo service) => {
    if(pass.Equals(service.getPassByEmpId(empid)))
        return Results.Created("/expense", service.setExpenseStatus(expid, status));
    return Results.Unauthorized();
    });

app.Run();

