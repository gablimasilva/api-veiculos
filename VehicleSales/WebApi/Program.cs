using Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApi.Extensions;
using WebApi.Handlers;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Services.AddProjectDependencies(
    builder.Configuration);

builder.Services.AddSwaggerDocumentation();

builder.Services.AddApiHealthChecks(
    builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddAuthentication("Dev")
        .AddScheme<AuthenticationSchemeOptions, DevAuthenticationHandler>(
            "Dev",
            null);
}
else
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            // Cognito
        });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext =
        scope.ServiceProvider.GetRequiredService<AppDataContext>();

    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();