using MeuDinheiro.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Banco;
using Shared.Dados.Modelos;
using Shared.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MDContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MDContext")));

builder.Services
    .AddIdentityApiEndpoints<PessoaComAcesso>()
    .AddEntityFrameworkStores<MDContext>();

builder.Services.AddAuthorization();

builder.Services.AddTransient<DAO<Receitas>>();
builder.Services.AddTransient<DAO<Despesas>>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCors(options => options.AddPolicy("wasm", policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:7089", builder.Configuration["FrontendUrl"] ?? "https://localhost:7015"]).AllowAnyMethod().SetIsOriginAllowed(pol => true).AllowAnyHeader().AllowCredentials()));

var app = builder.Build();

app.UseCors("wasm");

app.UseStaticFiles();
app.UseAuthorization();

app.AddDespesasEndpoints();
app.AddReceitasEndpoints();

app.MapGroup("auth").MapIdentityApi<PessoaComAcesso>().WithTags("Autorização");

app.MapPost("auth/logout", async ([FromServices] SignInManager<PessoaComAcesso> signInManager) =>
{
    await signInManager.SignOutAsync();
    Results.Ok();
}).RequireAuthorization().WithTags("Autorização");

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
        await context.Response.WriteAsync("Não autorizado");
        await context.Response.CompleteAsync();
    }
    else if (context.Response.StatusCode == 403)
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("Credenciais inválidas");
        await context.Response.CompleteAsync();
    }
});

app.Run();
