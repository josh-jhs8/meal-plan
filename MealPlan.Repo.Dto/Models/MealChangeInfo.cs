using MealPlan.Repo.Dto.Enums;

namespace MealPlan.Repo.Dto.Models
{
    public class MealChangeInfo
    {
        public AddOrRemove AddOrRemove { get; set; }
        public RecipeIngredientInfo Ingredient { get; set; }
    }
}
