using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Dto.Repos
{
    public interface IRecipeLogicRepo
    {
        Task<IngredientInfo[]> GetIngredients(string[] names);

        Task<IngredientInfo[]> GetIngredients();

        Task<RecipeInfo[]> GetRecipes();

        Task<RecipeInfo?> GetRecipe(string name);

        Task SaveRecipes(RecipeInfo[] recipes);

        Task DeleteRecipe(string name);
    }
}
