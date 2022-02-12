using MealPlan.Logic.DomainObject.Models;
using MealPlan.Repo.Dto.Models;

namespace MealPlan.Logic.DomainObject.Logics
{
    public interface IRecipeLogic
    {
        Task<ResultInfo> DeleteRecipe(string name);

        Task<ResultInfo> AddRecipe(RecipeInfo recipeInfo);

        Task<ResultInfo> LoadRecipesFromFile(string filePath);

        Task<ResultInfo> ExportRecipesToFile(string filePath);
    }
}
