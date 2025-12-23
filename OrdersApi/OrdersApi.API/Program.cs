using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Application.Interfaces;
using OrdersApi.Application.Orders.CreateOrder;
using OrdersApi.Infrastructure.Persistence;
using OrdersApi.Infrastructure.Persistence.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(kvp => kvp.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            code = "validation_error",
            message = "Validation failed.",
            errors
        });
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core (SQLite)
builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("OrdersDb");
    options.UseSqlite(cs);
});


// Bind Application abstraction to Infrastructure implementation
builder.Services.AddScoped<IOrdersDbContext>(sp => sp.GetRequiredService<OrdersDbContext>());

// MediatR (scan Application assembly)
builder.Services.AddMediatR(typeof(CreateOrderCommand).Assembly);

// FluentValidation (scan Application assembly)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(OrdersApi.Application.Orders.CreateOrder.CreateOrderValidator).Assembly);

builder.Services.AddScoped<OrdersApi.API.Middleware.ExceptionHandlingMiddleware>();

builder.Services.AddScoped<IIdempotencyStore, IdempotencyStore>();

var app = builder.Build();

app.UseMiddleware<OrdersApi.API.Middleware.ExceptionHandlingMiddleware>();

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
