using MealPlan.Db.Model;
using MealPlan.Repo.Dto.Models;

namespace MealPlan.Repo.Mapping
{
    internal static class RecipeMapping
    {
        internal static RecipeInfo MapToRecipeInfo(Recipe recipe)
        {
            return new RecipeInfo
            {
                Name = recipe.Name,
                Ingredients = recipe.RecipeIngredients.Select(ri => new RecipeIngredientInfo
                {
                    Name = ri.IngredientReference.Name,
                    Amount = ri.Amount,
                    UnitName = ri.IngredientReference.UnitName
                }).ToArray(),
                Steps = recipe.RecipeSteps
                    .OrderBy(rs => rs.StepNumber)
                    .Select(rs => rs.Text)
                    .ToArray(),
            };
        }

        internal static Recipe MapToRecipe(RecipeInfo recipe, IngredientReference[] ingredients)
        {
            return new Recipe
            {
                Name = recipe.Name.ToLower(),
                RecipeSteps = recipe.Steps.MapToRecipeSteps(),
                RecipeIngredients = recipe.Ingredients.Select(i => MapToRecipeIngredient(i, ingredients)).ToArray()
            };
        }

        public static RecipeStep[] MapToRecipeSteps(this string[] steps)
        {
            var output = new List<RecipeStep>();
            var stepNumber = 1;
            foreach (var step in steps) 
            { 
                output.Add(new RecipeStep { Text = step, StepNumber = stepNumber }); 
                stepNumber++; 
            }
            return output.ToArray();
        }

        public static RecipeIngredient MapToRecipeIngredient(RecipeIngredientInfo recipeIngredient, IngredientReference[] ingredients)
        {
            var referencedIngredient = ingredients.First(i => i.Name == recipeIngredient.Name.ToLower());
            return new RecipeIngredient
            {
                IngredientReferenceId = referencedIngredient.Id,
                Amount = recipeIngredient.Amount
            };
        }
    }
}
