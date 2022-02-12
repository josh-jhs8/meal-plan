using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Dto.Repos
{
    public interface IRecipeControllerRepo
    {
        Task<RecipeInfo[]> GetRecipes();

        Task<RecipeInfo?> GetRecipe(string name);
    }
}
