using MealPlan.Logic.DomainObject.Models;
using MealPlan.Repo.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan.Logic.DomainObject.Logics
{
    public interface IMealLogic
    {
        public Task<ResultInfo> AddMeal(RawMealInfo meal);

        public Task<FullMealInfo[]> GetMeals(DateTime? startDate, DateTime? endDate);

        public Task<ResultInfo> DeleteMeal(DateTime date, string recipe, string? mealType);

        public Task<ResultInfo> ExportRawMealsToFile(string filePath, DateTime? startDate, DateTime? endDate);

        public Task<ResultInfo> ExportFullMealsToFile(string filePath, DateTime? startDate, DateTime? endDate);

        public Task<ResultInfo> LoadRawMealsFromFile(string filePath);
    }
}
