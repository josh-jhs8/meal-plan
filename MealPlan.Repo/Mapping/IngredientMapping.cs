using MealPlan.Db.Model;
using MealPlan.Repo.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan.Repo.Mapping
{
    internal static class IngredientMapping
    {
        internal static IngredientInfo MapToIngredientInfo(IngredientReference ingredient)
        {
            return new IngredientInfo
            {
                Name = ingredient.Name,
                PerAmount = ingredient.PerAmount,
                UnitName = ingredient.UnitName,
                Protein = ingredient.Protein,
                Carbs = ingredient.Carbs,
                Fats = ingredient.Fat,
                Calories = ingredient.Calories
            };
        }

        internal static IngredientReference MapToIngredientReference(IngredientInfo ingredient)
        {
            return new IngredientReference
            {
                Name = ingredient.Name.ToLower(),
                PerAmount = ingredient.PerAmount,
                UnitName = ingredient.UnitName,
                Protein = ingredient.Protein,
                Carbs = ingredient.Carbs,
                Fat = ingredient.Fats,
                Calories = ingredient.Calories
            };
        }

        internal static void UpdateIngredientReferenceFromIngredientInfo(IngredientReference ingredient, IngredientInfo info)
        {
            ingredient.PerAmount = info.PerAmount;
            ingredient.UnitName = info.UnitName;
            ingredient.Protein = info.Protein;
            ingredient.Carbs = info.Carbs;
            ingredient.Fat = info.Fats;
            ingredient.Calories = info.Calories;
        }
    }
}
