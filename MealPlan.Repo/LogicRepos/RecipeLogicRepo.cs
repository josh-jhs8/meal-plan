using MealPlan.Db.Data;
using MealPlan.Db.Model;
using MealPlan.Repo.ControllerRepos;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using MealPlan.Repo.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MealPlan.Repo.LogicRepos
{
    public class RecipeLogicRepo : RecipeControllerRepo, IRecipeLogicRepo
    {
        public RecipeLogicRepo(MealPlanDb db) : base(db)
        {
        }

        public async Task DeleteRecipe(string name)
        {
            var recipe = await Db.Recipes
                .Include(r => r.RecipeIngredients)
                .Include(r => r.RecipeSteps)
                .FirstAsync(r => r.Name == name);
            Db.RecipeSteps.RemoveRange(recipe.RecipeSteps);
            Db.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);
            Db.Recipes.Remove(recipe);
            await Db.SaveChangesAsync();
        }

        public async Task SaveRecipes(RecipeInfo[] recipes)
        {
            var existingRecipes = (await Db.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.IngredientReference)
                .Include(r => r.RecipeSteps)
                .ToArrayAsync())
                .Where(a => recipes.Any(b => a.Name == b.Name))
                .ToArray();

            var ingredients = await Db.IngredientReferences.ToArrayAsync();

            foreach (var recipe in recipes)
            {
                var existingRecipe = existingRecipes.FirstOrDefault(er => er.Name == recipe.Name);
                if (existingRecipe == null)
                {
                    var newRecipe = RecipeMapping.MapToRecipe(recipe, ingredients);
                    await Db.Recipes.AddAsync(newRecipe);
                }
                else
                {
                    UpdateRecipeSteps(existingRecipe.RecipeSteps, recipe.Steps);
                    UpdateRecipeIngredients(existingRecipe.RecipeIngredients, recipe.Ingredients, ingredients);
                }
            }

            await Db.SaveChangesAsync();
        }

        private void UpdateRecipeSteps(ICollection<RecipeStep> recipeSteps, string[] infoSteps)
        {
            var stepNumber = 1;
            foreach (var step in infoSteps)
            {
                var recipeStep = recipeSteps.FirstOrDefault(rs => rs.StepNumber == stepNumber);
                if (recipeStep != null)
                {
                    recipeStep.Text = step;
                }
                else
                {
                    recipeStep = new RecipeStep { Text = step, StepNumber = stepNumber };
                    recipeSteps.Add(recipeStep);
                }
                stepNumber++;
            }

            if (recipeSteps.Count() > infoSteps.Length)
            {
                var excessRecipeSteps = recipeSteps.Where(rs => rs.StepNumber > infoSteps.Length);
                foreach (var recipeStep in excessRecipeSteps)
                {
                    recipeSteps.Remove(recipeStep);
                    Db.RecipeSteps.Remove(recipeStep);
                }
            }
        }

        private void UpdateRecipeIngredients(ICollection<RecipeIngredient> recipeIngredients, 
            RecipeIngredientInfo[] recipeIngredientInfos, IngredientReference[] ingredientReferences)
        {
            var recipeIngredientsToDelete = recipeIngredients
                .Where(ri => !recipeIngredientInfos.Any(info => ri.IngredientReference.Name == info.Name))
                .ToArray();

            foreach (var recipeIngredientToDelete in recipeIngredientsToDelete)
            {
                recipeIngredients.Remove(recipeIngredientToDelete);
                Db.RecipeIngredients.Remove(recipeIngredientToDelete);
            }

            foreach (var info in recipeIngredientInfos)
            {
                var existingRecipeIngredient = recipeIngredients.FirstOrDefault(ri => ri.IngredientReference.Name == info.Name);
                if (existingRecipeIngredient != null)
                {
                    existingRecipeIngredient.Amount = info.Amount;
                }
                else
                {
                    var ingredient = ingredientReferences.First(ir => ir.Name == info.Name);
                    var newRecipeIngredient = new RecipeIngredient
                    {
                        IngredientReferenceId = ingredient.Id,
                        Amount = info.Amount
                    };
                    recipeIngredients.Add(newRecipeIngredient);
                }
            }
        }

        public async Task<IngredientInfo[]> GetIngredients()
        {
            var ingredients = await Db.IngredientReferences
                .ToArrayAsync();

            var mapped = ingredients
                .Select(IngredientMapping.MapToIngredientInfo)
                .ToArray();

            return mapped;
        }

        public async Task<IngredientInfo[]> GetIngredients(string[] names)
        {
            var ingredients = await Db.IngredientReferences
                .Where(ir => names.Contains(ir.Name))
                .ToArrayAsync();

            var mapped = ingredients
                .Select(IngredientMapping.MapToIngredientInfo)
                .ToArray();

            return mapped;
        }
    }
}
