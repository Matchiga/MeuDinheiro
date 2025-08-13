using MyMoney.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Bank;
using Shared.Data.Models;
using Shared.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MDContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MDContext")));

builder.Services
    .AddIdentityApiEndpoints<PersonWithAccess>()
    .AddEntityFrameworkStores<MDContext>();

builder.Services.AddAuthorization();

builder.Services.AddTransient<DAO<Revenues>>();
builder.Services.AddTransient<DAO<Cost>>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCors(options => options.AddPolicy("wasm", policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:7089", builder.Configuration["FrontendUrl"] ?? "https://localhost:7015"]).AllowAnyMethod().SetIsOriginAllowed(pol => true).AllowAnyHeader().AllowCredentials()));

var app = builder.Build();

app.UseCors("wasm");

app.UseStaticFiles();
app.UseAuthorization();

app.AddCostEndpoints();
app.AddRevenuesEndpoints();

app.MapGroup("auth").MapIdentityApi<PersonWithAccess>().WithTags("Authorization");

app.MapPost("auth/logout", async ([FromServices] SignInManager<PersonWithAccess> signInManager) =>
{
    await signInManager.SignOutAsync();
    Results.Ok();
}).RequireAuthorization().WithTags("Authorization");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401)
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("Unauthorized");
        await context.Response.CompleteAsync();
    }
    else if (context.Response.StatusCode == 403)
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("Invalid credentials");
        await context.Response.CompleteAsync();
    }
});

app.Run();
