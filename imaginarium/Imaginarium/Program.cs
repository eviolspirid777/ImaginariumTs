using Imaginarium.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<UsersContext>(options => options.UseNpgsql("Server=localhost;Database=postgres;Port=5432;UserId=postgres;Password=94monizi;"));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors(builder => builder
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
