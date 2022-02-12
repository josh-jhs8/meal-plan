using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Dto.Repos
{
    public interface IIngredientControllerRepo
    {
        Task<IngredientInfo[]> GetIngredientReferences();

        Task<IngredientInfo?> GetIngredientReference(string name);
    }
}
