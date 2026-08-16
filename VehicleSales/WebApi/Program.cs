using Application;
using Infrastructure;
using Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApi.Extensions;
using WebApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Services.AddSwaggerDocumentation();

builder.Services.AddApiHealthChecks(
    builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "Dev";
            options.DefaultChallengeScheme = "Dev";
        })
        .AddScheme<AuthenticationSchemeOptions, DevAuthenticationHandler>(
            "Dev",
            null);
}
else
{
    builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority =
            builder.Configuration["Authentication:Authority"];

        options.Audience =
            builder.Configuration["Authentication:Audience"];

        options.MapInboundClaims = false;
    });
}

builder.Services.AddAuthorization();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();