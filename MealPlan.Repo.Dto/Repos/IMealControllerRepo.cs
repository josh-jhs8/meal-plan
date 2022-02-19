using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Dto.Repos
{
    public interface IMealControllerRepo
    {
        public Task<RawMealInfo[]> GetMeals(DateTime? startDate = null, DateTime? endDate = null);
    }
}
