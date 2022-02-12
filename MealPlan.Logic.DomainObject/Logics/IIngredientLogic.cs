using MealPlan.Logic.DomainObject.Models;
using MealPlan.Repo.Dto.Models;

namespace MealPlan.Logic.DomainObject.Logics
{
    public interface IIngredientLogic
    {
        Task<ResultInfo> DeleteIngredient(string name);

        Task<ResultInfo> AddIngredient(IngredientInfo ingredient);

        Task<ResultInfo> LoadIngredientsFromFile(string filePath);

        Task<ResultInfo> ExportIngredientsToFile(string filePath);
    }
}
