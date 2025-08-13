using MyMoney.Request;
using MyMoney.Response;
using Microsoft.AspNetCore.Mvc;
using Shared.Bank;
using Shared.Models;

namespace MyMoney.Endpoints;

public static class RevenuesEndpoint
{
    public static void AddRevenuesEndpoints(this WebApplication app)
    {
        var groupBuilder = app.MapGroup("revenues")
            .RequireAuthorization()
            .WithTags("Revenues");

        groupBuilder.MapGet("", ([FromServices] DAO<Revenues> dao) =>
        {
            var recipeList = dao.List();
            if (recipeList is null)
            {
                return Results.NotFound();
            }
            var recipeListResponse = EntityListToResponseList(recipeList);
            return Results.Ok(recipeListResponse);
        });

        groupBuilder.MapGet("/buscar", ([FromServices] DAO<Revenues> dao, string? description = "") =>
        {
            IEnumerable<Revenues> recipeList = dao.List();

            if (!string.IsNullOrEmpty(description))
            {
                recipeList = recipeList.Where(d => d.Description.Contains(description, StringComparison.OrdinalIgnoreCase));
            }

            if (recipeList == null || !recipeList.Any())
            {
                return Results.NotFound();
            }

            var recipeListResponse = EntityListToResponseList(recipeList);
            return Results.Ok(recipeListResponse);
        });

        groupBuilder.MapGet("{id}", ([FromServices] DAO<Revenues> dao, int id) =>
        {
            var recipes = dao.RecoverBy(a => a.Id == id);
            if (recipes is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(EntityToResponse(recipes));
        });

        groupBuilder.MapGet("/{year:int}/{month:int}", ([FromServices] DAO<Revenues> dao, int year, int month) =>
        {
            var recipesoftheMonth = dao.List().Where(d => d.Date.Year == year && d.Date.Month == month);

            if (!recipesoftheMonth.Any())
            {
                return Results.NotFound();
            }

            var recipesResponse = EntityListToResponseList(recipesoftheMonth);
            return Results.Ok(recipesResponse);
        });

        groupBuilder.MapPost("", ([FromServices] DAO<Revenues> dao, [FromBody] RevenueRequest recipesRequest) =>
        {
            var recipes = new Revenues(recipesRequest.Id, recipesRequest.Description, recipesRequest.Value, recipesRequest.Date);

            if (recipesRequest.Description is null)
            {
                return Results.BadRequest();
            }

            dao.Add(recipes);
            return Results.Ok(EntityToResponse(recipes));
        });

        groupBuilder.MapDelete("{id}", ([FromServices] DAO<Revenues> dao, int id) =>
        {
            var categories = dao.RecoverBy(a => a.Id == id);
            if (categories is null)
            {
                return Results.NotFound();
            }
            dao.Remove(categories);
            return Results.Ok();
        });

        groupBuilder.MapPut("", ([FromServices] DAO<Revenues> dao, [FromBody] RevenueRequest recipesRequest) =>
        {
            var recipesUpdate = dao.RecoverBy(a => a.Id == recipesRequest.Id);
            if (recipesUpdate is null)
            {
                return Results.NotFound();
            }
            recipesUpdate.Description = recipesRequest.Description;
            recipesUpdate.Value = recipesRequest.Value;

            dao.Update(recipesUpdate);
            return Results.Ok();
        });
    }

    private static ICollection<RevenueResponse> EntityListToResponseList(IEnumerable<Revenues> recipeList)
    {
        return recipeList.Select(a => EntityToResponse(a)).ToList();
    }

    private static RevenueResponse EntityToResponse(Revenues recipes)
    {
        return new RevenueResponse(recipes.Id, recipes.Description, recipes.Value, recipes.Date);
    }
}
