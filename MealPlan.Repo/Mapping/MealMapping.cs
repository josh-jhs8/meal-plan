using MealPlan.Db.Model;
using MealPlan.Repo.Dto.Enums;
using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Mapping
{
    internal static class MealMapping
    {
        internal static RawMealInfo MapToRawMealInfo(Meal meal)
        {
            return new RawMealInfo
            {
                Date = meal.PlannedDate,
                Recipe = meal.Recipe.Name,
                Type = meal.MealType.Name,
                Changes = meal.MealChanges.Select(MapToMealChangeInfo).ToArray()
            };
        }

        internal static MealChangeInfo MapToMealChangeInfo(MealChange change)
        {
            return new MealChangeInfo
            {
                AddOrRemove = change.AddOrRemove == "ADD" ? AddOrRemove.Add : AddOrRemove.Remove,
                Ingredient = new RecipeIngredientInfo
                {
                    Amount = change.Amount,
                    Name = change.IngredientReference.Name,
                    UnitName = change.IngredientReference.UnitName
                }
            };
        }
    }
}
