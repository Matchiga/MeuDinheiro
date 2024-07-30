using Shared.Banco;
using Shared.Modelos;
using Shared.Dados.Modelos;
using Microsoft.AspNetCore.Mvc;
using MeuDinheiro.Response;
using System.Runtime.CompilerServices;
using MeuDinheiro.Request;

namespace MeuDinheiro.Endpoints;

public static class DespesasEndpoint
{
    public static void AddDespesasEndpoints(this WebApplication app)
    {
        var groupBuilder = app.MapGroup("despesas")
            .RequireAuthorization()
            .WithTags("Despesas");

        groupBuilder.MapGet("", ([FromServices] DAO<Despesas> dao) =>
        {
            var listaDeDespesas = dao.Listar();
            if (listaDeDespesas is null)
            {
                return Results.NotFound();
            }
            var listaDeDespesasResponse = EntityListToResponseList(listaDeDespesas);
            return Results.Ok(listaDeDespesasResponse);
        });

        groupBuilder.MapGet("{id}", ([FromServices] DAO<Despesas> dao, int id) =>
        {
            var despesas = dao.RecuperarPor(a => a.Id == id);
            if (despesas is null)
            {
                return Results.NotFound();
            }
            var despesasResponse = EntityToResponse(despesas);
            return Results.Ok(despesasResponse);
        });

        groupBuilder.MapGet("/buscar", ([FromServices] DAO<Despesas> dao, string? descricao = "") =>
        {
            IEnumerable<Despesas> listaDeDespesas = dao.Listar();

            if (!string.IsNullOrEmpty(descricao))
            {
                listaDeDespesas = listaDeDespesas.Where(d => d.Descricao.Contains(descricao, StringComparison.OrdinalIgnoreCase));
            }

            if (listaDeDespesas == null || !listaDeDespesas.Any())
            {
                return Results.NotFound();
            }

            var listaDeDespesasResponse = EntityListToResponseList(listaDeDespesas);
            return Results.Ok(listaDeDespesasResponse);
        });

        groupBuilder.MapGet("/{ano:int}/{mes:int}", ([FromServices] DAO<Despesas> dao, int ano, int mes) =>
        {
            var despesasDoMes = dao.Listar().Where(d => d.Data.Year == ano && d.Data.Month == mes);

            if (!despesasDoMes.Any())
            {
                return Results.NotFound();
            }

            var despesasResponse = EntityListToResponseList(despesasDoMes);
            return Results.Ok(despesasResponse);
        });

        groupBuilder.MapPost("", ([FromServices] DAO<Despesas> dal, [FromBody] DespesaRequest despesaRequest) =>
        {
            var despesas = new Despesas(despesaRequest.Id, despesaRequest.Descricao, despesaRequest.Valor, despesaRequest.Data);

            if (despesaRequest.Descricao is null)
            {
                return Results.BadRequest();
            }

            dal.Adicionar(despesas);
            return Results.Ok(EntityToResponse(despesas));
        });

        groupBuilder.MapPut("", ([FromServices] DAO<Despesas> dao, [FromBody] DespesaRequest despesasRequest) =>
        {
            var categoriasAtualizar = dao.RecuperarPor(a => a.Id == despesasRequest.Id);
            if (categoriasAtualizar is null)
            {
                return Results.NotFound();
            }
            categoriasAtualizar.Descricao = despesasRequest.Descricao;
            categoriasAtualizar.Valor = despesasRequest.Valor;

            dao.Atualizar(categoriasAtualizar);
            return Results.Ok();
        });

        groupBuilder.MapDelete("{id}", ([FromServices] DAO<Despesas> dal, int id) =>
        {
            var despesas = dal.RecuperarPor(a => a.Id == id);
            if (despesas is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(despesas);
            return Results.Ok();
        });

        groupBuilder.MapGet("/resumo/{ano:int}/{mes:int}",([FromServices] DAO<Despesas> daoDespesas,[FromServices]DAO<Receitas> daoReceitas, int ano, int mes) =>
        {
            var despesasDoMes = daoDespesas.Listar().Where(d => d.Data.Year == ano && d.Data.Month == mes);
            var receitasDoMes = daoReceitas.Listar().Where(r => r.Data.Year == ano && r.Data.Month == mes);

            var resumo = new ResumoResponse
            {
                TotalReceitas = receitasDoMes.Sum(r => r.Valor),
                TotalDespesas = despesasDoMes.Sum(d => d.Valor),
                SaldoFinal = receitasDoMes.Sum(r => r.Valor) - despesasDoMes.Sum(d => d.Valor), GastosPorCategoria = despesasDoMes.GroupBy(d => d.Categoria?.Nome ?? "Sem Categoria").ToDictionary(g => g.Key, g => g.Sum(d => d.Valor))
            };

           return Results.Ok(resumo);
        });
    }

    private static ICollection<DespesaResponse> EntityListToResponseList(IEnumerable<Despesas> listaDeCategorias)
    {
        return listaDeCategorias.Select(a => EntityToResponse(a)).ToList();
    }

    private static DespesaResponse EntityToResponse(Despesas despesas)
    {
        return new DespesaResponse(despesas.Id, despesas.Descricao, despesas.Valor, despesas.Data, despesas.CategoriaId, despesas.Categoria?.Nome ?? "");
    }
}
