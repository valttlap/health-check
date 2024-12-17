using HealthCheck.API.Extensions;
using HealthCheck.API.Hubs;
using HealthCheck.Model.Context;

using Microsoft.EntityFrameworkCore;

using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
var connStringBuilder = new NpgsqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));

var connString = connStringBuilder.ConnectionString;

var datasourceBuilder = new NpgsqlDataSourceBuilder(connString);

var datasource = datasourceBuilder.Build();

builder.Services.AddDbContext<HealthCheckContext>(opt => opt.UseNpgsql(datasource));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();

app.MapControllers();
app.MapHub<HealthCheckHub>("hubs/health-check-hub");

app.Run();
