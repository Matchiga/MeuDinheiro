using Shared.Bank;
using Shared.Models;
using Shared.Data.Models;
using Microsoft.AspNetCore.Mvc;
using MyMoney.Response;
using System.Runtime.CompilerServices;
using MyMoney.Request;

namespace MyMoney.Endpoints;

public static class DespesasEndpoint
{
    public static void AddCostEndpoints(this WebApplication app)
    {
        var groupBuilder = app.MapGroup("Costs")
            .RequireAuthorization()
            .WithTags("Costs");

        groupBuilder.MapGet("", ([FromServices] DAO<Cost> dao) =>
        {
            var costList = dao.List();
            if (costList is null)
            {
                return Results.NotFound();
            }
            var costListResponse = EntityListToResponseList(costList);
            return Results.Ok(costListResponse);
        });

        groupBuilder.MapGet("{id}", ([FromServices] DAO<Cost> dao, int id) =>
        {
            var cost = dao.RecoverBy(a => a.Id == id);
            if (cost is null)
            {
                return Results.NotFound();
            }
            var costResponse = EntityToResponse(cost);
            return Results.Ok(costResponse);
        });

        groupBuilder.MapGet("/search", ([FromServices] DAO<Cost> dao, string? description = "") =>
        {
            IEnumerable<Cost> costList = dao.List();

            if (!string.IsNullOrEmpty(description))
            {
                costList = costList.Where(d => d.Description.Contains(description, StringComparison.OrdinalIgnoreCase));
            }

            if (costList == null || !costList.Any())
            {
                return Results.NotFound();
            }

            var costListResponse = EntityListToResponseList(costList);
            return Results.Ok(costListResponse);
        });

        groupBuilder.MapGet("/{year:int}/{month:int}", ([FromServices] DAO<Cost> dao, int year, int month) =>
        {
            var monthlyCostList = dao.List().Where(d => d.Date.Year == year && d.Date.Month == month);

            if (!monthlyCostList.Any())
            {
                return Results.NotFound();
            }

            var costResponse = EntityListToResponseList(monthlyCostList);
            return Results.Ok(costResponse);
        });

        groupBuilder.MapPost("", ([FromServices] DAO<Cost> dal, [FromBody] CostRequest costRequest) =>
        {
            var cost = new Cost(costRequest.Id, costRequest.Description, costRequest.Value, costRequest.Date);

            if (costRequest.Description is null)
            {
                return Results.BadRequest();
            }

            dal.Add(cost);
            return Results.Ok(EntityToResponse(cost));
        });

        groupBuilder.MapPut("", ([FromServices] DAO<Cost> dao, [FromBody] CostRequest costRequest) =>
        {
            var categoriesUpdate = dao.RecoverBy(a => a.Id == costRequest.Id);
            if (categoriesUpdate is null)
            {
                return Results.NotFound();
            }
            categoriesUpdate.Description = costRequest.Description;
            categoriesUpdate.Value = costRequest.Value;

            dao.Update(categoriesUpdate);
            return Results.Ok();
        });

        groupBuilder.MapDelete("{id}", ([FromServices] DAO<Cost> dal, int id) =>
        {
            var cost = dal.RecoverBy(a => a.Id == id);
            if (cost is null)
            {
                return Results.NotFound();
            }
            dal.Remove(cost);
            return Results.Ok();
        });

        groupBuilder.MapGet("/summary/{year:int}/{month:int}",([FromServices] DAO<Cost> daoCost,[FromServices]DAO<Revenues> daoRecipes, int year, int month) =>
        {
            var monthlyCostList = daoCost.List().Where(d => d.Date.Year == year && d.Date.Month == month);
            var recipesOfTheMonth = daoRecipes.List().Where(r => r.Date.Year == year && r.Date.Month == month);

            var summary = new SummaryResponse
            {
                TotalRevenues = recipesOfTheMonth.Sum(r => r.Value),
                TotalCost = monthlyCostList.Sum(d => d.Value),
                FinalBalance = recipesOfTheMonth.Sum(r => r.Value) - monthlyCostList.Sum(d => d.Value), ExpensesByCategory = monthlyCostList.GroupBy(d => d.Category?.Name ?? "Uncategorized").ToDictionary(g => g.Key, g => g.Sum(d => d.Value))
            };

           return Results.Ok(summary);
        });
    }

    private static ICollection<CostResponse> EntityListToResponseList(IEnumerable<Cost> categoryList)
    {
        return categoryList.Select(a => EntityToResponse(a)).ToList();
    }

    private static CostResponse EntityToResponse(Cost cost)
    {
        return new CostResponse(cost.Id, cost.Description, cost.Value, cost.Date, cost.CategoryId, cost.Category?.Name ?? "");
    }
}
