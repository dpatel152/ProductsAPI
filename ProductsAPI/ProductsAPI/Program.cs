﻿using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<INotifyService, NotifyService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=products.db")); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

