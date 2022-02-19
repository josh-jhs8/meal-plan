using MealPlan.Logic.DomainObject.Models;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan.Logic
{
    public class MacroCalculatorLogic
    {
        private readonly IMacroCalculatorLogicRepo _repo;

        public MacroCalculatorLogic(IMacroCalculatorLogicRepo repo)
        {
            _repo = repo;
        }

        protected async Task<MacroInfo> CalculateMacrosForRecipeIngredients(RecipeIngredientInfo[] recipeIngredients)
        {
            var ingredientList = recipeIngredients.Select(x => x.Name).ToArray();
            var ingredients = await _repo.GetIngredients(ingredientList);

            var macroInfo = new MacroInfo();
            foreach (var recipeIngredient in recipeIngredients)
            {
                var ingredient = ingredients.First(i => i.Name == recipeIngredient.Name);
                macroInfo.Carbs += recipeIngredient.Amount * (ingredient.Carbs / ingredient.PerAmount);
                macroInfo.Fats += recipeIngredient.Amount * (ingredient.Fats / ingredient.PerAmount);
                macroInfo.Protein += recipeIngredient.Amount * (ingredient.Protein / ingredient.PerAmount);
                macroInfo.Calories += (int)(recipeIngredient.Amount * (ingredient.Calories / ingredient.PerAmount));
            }

            return macroInfo;
        }
    }
}
