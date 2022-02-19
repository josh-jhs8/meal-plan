using MealPlan.Logic.DomainObject.Logics;
using MealPlan.Logic.DomainObject.Models;
using MealPlan.Repo.Dto.Enums;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan.Logic
{
    public class MealLogic : MacroCalculatorLogic, IMealLogic
    {
        private readonly IMealLogicRepo _repo;

        public MealLogic(IMealLogicRepo repo) : base(repo)
        {
            _repo = repo;
        }

        public async Task<ResultInfo> AddMeal(RawMealInfo meal)
        {
            var recipe = await _repo.GetRecipe(meal.Recipe);
            if (recipe == null) return new ResultInfo { Message = $"{meal.Recipe} does not exist in recipes", Success = false };

            var ingredientList = meal.Changes.Select(c => c.Ingredient.Name).ToList();
            var ingredients = await _repo.GetIngredients(ingredientList.ToArray());
            ingredientList.RemoveAll(a => ingredients.Any(b => b.Name == a));
            if (ingredients.Count() > 0)
            {
                return new ResultInfo { Message = $"The following ingredients do not exist in reference: {string.Join(", ", ingredientList)}", Success = false };
            }

            var ingredientsToRemove = meal.Changes.Where(c => c.AddOrRemove == AddOrRemove.Remove).Select(c => c.Ingredient).ToList();
            ingredientsToRemove.RemoveAll(ic => recipe.Ingredients.Any(i => i.Name == ic.Name));
            if (ingredientsToRemove.Count() > 0)
            {
                return new ResultInfo { Message = $"The following ingredients to remove aren't in the recipe: {string.Join(", ", ingredientsToRemove)}", Success = false };
            }

            await _repo.SaveMeals(new RawMealInfo[] { meal });
            return new ResultInfo { Message = $"{meal.Recipe} - {meal.Date.ToShortDateString()} Saved", Success = true };
        }

        public async Task<FullMealInfo[]> GetMeals(DateTime? startDate = null, DateTime? endDate = null)
        {
            var output = new List<FullMealInfo>();
            var meals = await _repo.GetMeals(startDate, endDate);

            foreach (var meal in meals)
            {
                var recipe = await _repo.GetRecipe(meal.Recipe);
                if (recipe == null) throw new Exception($"Could not find {meal.Recipe} in recipes ???");

                var recipeIngredients = GetAdjustedRecipeIngredients(recipe.Ingredients, meal.Changes);
                var macros = await CalculateMacrosForRecipeIngredients(recipeIngredients);

                output.Add(new FullMealInfo
                {
                    Date = meal.Date,
                    Name = meal.Recipe,
                    Type = meal.Type,
                    Ingredients = recipeIngredients,
                    Steps = recipe.Steps,
                    Macros = macros
                });
            }

            return output.ToArray();
        }

        private RecipeIngredientInfo[] GetAdjustedRecipeIngredients(RecipeIngredientInfo[] recipeIngredients, MealChangeInfo[] changes)
        {
            var output = new List<RecipeIngredientInfo>();
            output.AddRange(recipeIngredients);
            foreach (var change in changes)
            {
                if (change.AddOrRemove == AddOrRemove.Add)
                {
                    output.Add(change.Ingredient);
                }
                else
                {
                    var toRemove = recipeIngredients.First(ri => ri.Name == change.Ingredient.Name && ri.Amount == change.Ingredient.Amount);
                    output.Remove(toRemove);
                }
            }
            return output.ToArray();
        }
    }
}
