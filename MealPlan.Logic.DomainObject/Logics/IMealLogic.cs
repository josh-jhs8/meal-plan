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

        public Task<FullMealInfo[]> GetMeals(DateTime? startDate = null, DateTime? endDate = null);
    }
}
