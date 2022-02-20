using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Dto.Repos
{
    public interface IMealLogicRepo : IMacroCalculatorLogicRepo
    {
        public Task SaveMeals(RawMealInfo[] meals);

        public Task<RawMealInfo[]> GetMeals(DateTime? startDate = null, DateTime? endDate = null);

        public Task DeleteMeal(RawMealInfo meal);

        public Task<string[]> GetRecipes();
    }
}
