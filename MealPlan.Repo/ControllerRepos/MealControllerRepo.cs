using MealPlan.Db.Data;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using MealPlan.Repo.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MealPlan.Repo.ControllerRepos
{
    public class MealControllerRepo : IMealControllerRepo
    {
        protected readonly MealPlanDb Db;

        public MealControllerRepo(MealPlanDb db)
        {
            Db = db;
        }

        public async Task<RawMealInfo[]> GetMeals(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = Db.Meals
                .Include(m => m.MealType)
                .Include(m => m.Recipe)
                .Include(m => m.MealChanges)
                .ThenInclude(m => m.IngredientReference)
                .AsQueryable();
                
            if (startDate != null) query = query.Where(m => m.PlannedDate >= startDate);
            if (endDate != null) query = query.Where(m => m.PlannedDate <= endDate);

            var meals = await query.ToArrayAsync();

            var mapped = meals.Select(MealMapping.MapToRawMealInfo).ToArray();
            return mapped;
        }
    }
}
