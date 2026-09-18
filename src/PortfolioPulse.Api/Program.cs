using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<PortfolioPulseDbContext>(options =>
        options.UseSqlite(
            builder.Configuration.GetConnectionString("PortfolioPulse")));
}
else
{
    builder.Services.AddDbContext<PortfolioPulseDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("PortfolioPulse")));
}

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IHoldingService, HoldingService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.MapControllers();

app.Run();
