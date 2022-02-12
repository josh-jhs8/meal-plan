using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Dto.Repos
{
    public interface IRecipeLogicRepo
    {
        Task<RecipeInfo[]> GetRecipes();

        Task<RecipeInfo?> GetRecipe(string name);

        Task SaveRecipes(RecipeInfo[] recipes);

        Task DeleteRecipe(string name);
    }
}
