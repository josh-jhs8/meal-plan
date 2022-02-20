using MealPlan.Db.Data;
using MealPlan.Db.Model;
using MealPlan.Repo.Dto.Enums;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using MealPlan.Repo.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MealPlan.Repo.LogicRepos
{
    public class MealLogicRepo : IMealLogicRepo
    {
        private readonly MealPlanDb _db;

        public MealLogicRepo(MealPlanDb db)
        {
            _db = db;
        }

        public async Task<IngredientInfo[]> GetIngredients(string[] names)
        {
            var ingredients = await _db.IngredientReferences
                .Where(ir => names.Contains(ir.Name))
                .ToArrayAsync();

            var mapped = ingredients
                .Select(IngredientMapping.MapToIngredientInfo)
                .ToArray();

            return mapped;
        }

        public async Task<RawMealInfo[]> GetMeals(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _db.Meals
                .Include(m => m.MealType)
                .Include(m => m.Recipe)
                .Include(m => m.MealChanges)
                .ThenInclude(m => m.IngredientReference)
                .AsQueryable();
                
            if (startDate != null) query = query.Where(m => m.PlannedDate >= startDate);
            if (endDate != null) query = query.Where(m => m.PlannedDate <= endDate);

            var meals = await query.ToArrayAsync();

            var mapped = meals.Select(MealMapping.MapToRawMealInfo).ToArray();
            return mapped;
        }

        public async Task<RecipeInfo?> GetRecipe(string name)
        {
            var recipe = await _db.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.IngredientReference)
                .Include(r => r.RecipeSteps)
                .FirstOrDefaultAsync(r => r.Name == name);
            if (recipe == null) return null;

            var mapped = RecipeMapping.MapToRecipeInfo(recipe);
            return mapped;
        }

        public async Task SaveMeals(RawMealInfo[] meals)
        {
            var recipeList = meals.Select(m => m.Recipe).ToArray();
            var recipes = (await _db.Recipes.ToArrayAsync()).Where(r => recipeList.Contains(r.Name)).ToArray();

            var ingredientList = meals.SelectMany(m => m.Changes).Select(c => c.Ingredient.Name).ToArray();
            var ingredients = (await _db.IngredientReferences.ToArrayAsync()).Where(i => ingredientList.Contains(i.Name)).ToArray();

            var mealTypes = await _db.MealTypes.ToArrayAsync();

            var existingMeals = (await _db.Meals
                .Include(m => m.Recipe)
                .Include(m => m.MealChanges)
                .ThenInclude(c => c.IngredientReference)
                .Include(m => m.MealType)
                .ToArrayAsync())
                .Where(em => meals.Any(m => m.Recipe == em.Recipe.Name && m.Date == em.PlannedDate && m.Type == em.MealType.Name))
                .ToArray();

            foreach (var meal in meals)
            {
                var databaseMeal = existingMeals.FirstOrDefault(em => meal.Recipe == em.Recipe.Name && meal.Date == em.PlannedDate && meal.Type == em.MealType.Name);
                if (databaseMeal == null)
                {
                    var recipe = recipes.First(r => r.Name == meal.Recipe);
                    var type = mealTypes.First(t => t.Name == meal.Type);
                    databaseMeal = new Meal
                    {
                        PlannedDate = meal.Date,
                        RecipeId = recipe.Id,
                        MealTypeId = type.Id,
                        MealChanges = new List<MealChange>()
                    };
                    _db.Meals.Add(databaseMeal);
                }
                UpdateMealChanges(databaseMeal.MealChanges, meal.Changes, ingredients);
            }

            await _db.SaveChangesAsync();
        }

        private void UpdateMealChanges(ICollection<MealChange> mealChanges, MealChangeInfo[] mealChangeInfos, IngredientReference[] ingredients)
        {
            var changesToDelete = mealChanges
                .Where(c => !mealChangeInfos.Any(mc => ConvertToAddOrRemove(c.AddOrRemove) == mc.AddOrRemove && c.IngredientReference.Name == mc.Ingredient.Name))
                .ToArray();

            foreach (var changeToDelete in changesToDelete)
            {
                mealChanges.Remove(changeToDelete);
                _db.MealChanges.Remove(changeToDelete);
            }

            foreach (var info in mealChangeInfos)
            {
                var existingChange = mealChanges.FirstOrDefault(c => ConvertToAddOrRemove(c.AddOrRemove) == info.AddOrRemove && c.IngredientReference.Name == info.Ingredient.Name);
                if (existingChange != null)
                {
                    existingChange.Amount = info.Ingredient.Amount;
                }
                else
                {
                    var ingredient = ingredients.First(i => i.Name == info.Ingredient.Name);
                    mealChanges.Add(new MealChange
                    {
                        AddOrRemove = info.AddOrRemove == AddOrRemove.Add ? "ADD" : "REMOVE",
                        Amount = info.Ingredient.Amount,
                        IngredientReferenceId = ingredient.Id,
                    });
                }
            }
                
        }

        private AddOrRemove ConvertToAddOrRemove(string addOrRemove)
        {
            if (addOrRemove == "ADD") return AddOrRemove.Add;
            if (addOrRemove == "REMOVE") return AddOrRemove.Remove;
            throw new Exception("Invalid Add or Remove data found");
        }

        public async Task DeleteMeal(RawMealInfo meal)
        {
            var entity = await _db.Meals
                .Include(m => m.MealChanges)
                .FirstAsync(m => m.PlannedDate == meal.Date && m.Recipe.Name == meal.Recipe && m.MealType.Name == meal.Type);

            _db.MealChanges.RemoveRange(entity.MealChanges);
            _db.Meals.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<string[]> GetRecipes()
        {
            var recipes = await _db.Recipes.Select(r => r.Name).ToArrayAsync();
            return recipes;
        }
    }
}
