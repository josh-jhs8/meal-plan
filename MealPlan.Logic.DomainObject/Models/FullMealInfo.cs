using MealPlan.Repo.Dto.Models;

namespace MealPlan.Logic.DomainObject.Models
{
    public class FullMealInfo
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public RecipeIngredientInfo[] Ingredients { get; set; }
        public string[] Steps { get; set; }
        public MacroInfo Macros { get; set; }
    }
}
