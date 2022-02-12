using MealPlan.Db.Data;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using MealPlan.Repo.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MealPlan.Repo.ControllerRepos
{
    public class RecipeControllerRepo : IRecipeControllerRepo
    {
        protected readonly MealPlanDb Db;

        public RecipeControllerRepo(MealPlanDb db)
        {
            Db = db;
        }

        public async Task<RecipeInfo?> GetRecipe(string name)
        {
            var recipe = await Db.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.IngredientReference)
                .Include(r => r.RecipeSteps)
                .FirstOrDefaultAsync(r => r.Name == name);
            if (recipe == null) return null;

            var mapped = RecipeMapping.MapToRecipeInfo(recipe);
            return mapped;
        }

        public async Task<RecipeInfo[]> GetRecipes()
        {
            var recipes = await Db.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.IngredientReference)
                .Include(r => r.RecipeSteps)
                .ToArrayAsync();
            var mapped = recipes.Select(RecipeMapping.MapToRecipeInfo).ToArray();
            return mapped;
        }


    }
}
