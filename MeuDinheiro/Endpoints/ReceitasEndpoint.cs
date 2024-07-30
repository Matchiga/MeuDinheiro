using MeuDinheiro.Request;
using MeuDinheiro.Response;
using Microsoft.AspNetCore.Mvc;
using Shared.Banco;
using Shared.Modelos;

namespace MeuDinheiro.Endpoints;

public static class ReceitasEndpoint
{
    public static void AddReceitasEndpoints(this WebApplication app)
    {
        var groupBuilder = app.MapGroup("receitas")
            .RequireAuthorization()
            .WithTags("Receitas");

        groupBuilder.MapGet("", ([FromServices] DAO<Receitas> dao) =>
        {
            var listaDeReceitas = dao.Listar();
            if (listaDeReceitas is null)
            {
                return Results.NotFound();
            }
            var listaDeReceitasResponse = EntityListToResponseList(listaDeReceitas);
            return Results.Ok(listaDeReceitasResponse);
        });

        groupBuilder.MapGet("/buscar", ([FromServices] DAO<Receitas> dao, string? descricao = "") =>
        {
            IEnumerable<Receitas> listaDeReceitas = dao.Listar();

            if (!string.IsNullOrEmpty(descricao))
            {
                listaDeReceitas = listaDeReceitas.Where(d => d.Descricao.Contains(descricao, StringComparison.OrdinalIgnoreCase));
            }

            if (listaDeReceitas == null || !listaDeReceitas.Any())
            {
                return Results.NotFound();
            }

            var listaDeReceitasResponse = EntityListToResponseList(listaDeReceitas);
            return Results.Ok(listaDeReceitasResponse);
        });

        groupBuilder.MapGet("{id}", ([FromServices] DAO<Receitas> dao, int id) =>
        {
            var receitas = dao.RecuperarPor(a => a.Id == id);
            if (receitas is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(EntityToResponse(receitas));
        });

        groupBuilder.MapGet("/{ano:int}/{mes:int}", ([FromServices] DAO<Receitas> dao, int ano, int mes) =>
        {
            var receitasDoMes = dao.Listar().Where(d => d.Data.Year == ano && d.Data.Month == mes);

            if (!receitasDoMes.Any())
            {
                return Results.NotFound();
            }

            var receitasResponse = EntityListToResponseList(receitasDoMes);
            return Results.Ok(receitasResponse);
        });

        groupBuilder.MapPost("", ([FromServices] DAO<Receitas> dao, [FromBody] ReceitaRequest receitasRequest) =>
        {
            var receitas = new Receitas(receitasRequest.Id, receitasRequest.Descricao, receitasRequest.Valor, receitasRequest.Data);

            if (receitasRequest.Descricao is null)
            {
                return Results.BadRequest();
            }

            dao.Adicionar(receitas);
            return Results.Ok(EntityToResponse(receitas));
        });

        groupBuilder.MapDelete("{id}", ([FromServices] DAO<Receitas> dao, int id) =>
        {
            var categorias = dao.RecuperarPor(a => a.Id == id);
            if (categorias is null)
            {
                return Results.NotFound();
            }
            dao.Deletar(categorias);
            return Results.Ok();
        });

        groupBuilder.MapPut("", ([FromServices] DAO<Receitas> dao, [FromBody] ReceitaRequest receitasRequest) =>
        {
            var receitasAtualizar = dao.RecuperarPor(a => a.Id == receitasRequest.Id);
            if (receitasAtualizar is null)
            {
                return Results.NotFound();
            }
            receitasAtualizar.Descricao = receitasRequest.Descricao;
            receitasAtualizar.Valor = receitasRequest.Valor;

            dao.Atualizar(receitasAtualizar);
            return Results.Ok();
        });
    }

    private static ICollection<ReceitaResponse> EntityListToResponseList(IEnumerable<Receitas> listaDeReceitas)
    {
        return listaDeReceitas.Select(a => EntityToResponse(a)).ToList();
    }

    private static ReceitaResponse EntityToResponse(Receitas receitas)
    {
        return new ReceitaResponse(receitas.Id, receitas.Descricao, receitas.Valor, receitas.Data);
    }
}
