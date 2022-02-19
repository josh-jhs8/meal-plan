using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Dto.Repos
{
    public interface IRecipeLogicRepo : IMacroCalculatorLogicRepo
    {
        Task<IngredientInfo[]> GetIngredients();

        Task<RecipeInfo[]> GetRecipes();

        Task SaveRecipes(RecipeInfo[] recipes);

        Task DeleteRecipe(string name);
    }
}
