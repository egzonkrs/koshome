using KosHome.Api.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using KosHome.Api.Extensions.DependencyInjection;
using KosHome.Api.Middlewares;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddModule(new CoreModule());
builder.Services.AddModule(new DataModule(builder.Configuration));
builder.Services.AddModule(new AuthModule(builder.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// CORS configuration for Next.js frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("NextJsFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(_ => {});
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("NextJsFrontend");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();