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

app.MapGet("/expense", ([FromQuery] int id, [FromServices] DatabaseRepo service) => {
    if(service.getPassByEmpId(id).Equals(pass)){
        return service.getExpensesByEmpId(id);
    }
    return "forbidden";
    });
app.Run();
