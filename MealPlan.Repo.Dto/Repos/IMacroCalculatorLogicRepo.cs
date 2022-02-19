using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Dto.Repos
{
    public interface IMacroCalculatorLogicRepo
    {
        Task<IngredientInfo[]> GetIngredients(string[] names);

        Task<RecipeInfo?> GetRecipe(string name);
    }
}
