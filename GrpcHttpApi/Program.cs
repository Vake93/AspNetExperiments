using Greeter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcHttpApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Greeter",
        Version = "v1"
    });
});

builder.Services.AddGrpcSwagger();

var app = builder.Build();

app.MapSwagger();

app.MapGrpcService<GreeterService>();

app.UseSwaggerUI();

app.Run();
