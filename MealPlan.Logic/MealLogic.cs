using MealPlan.Logic.DomainObject.Logics;
using MealPlan.Logic.DomainObject.Models;
using MealPlan.Repo.Dto.Enums;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using Newtonsoft.Json;

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

        public async Task<ResultInfo> DeleteMeal(DateTime date, string recipe, string? mealType)
        {
            var meals = await _repo.GetMeals(date, date);
            var mealToDelete = meals.FirstOrDefault(m => m.Recipe == recipe && (mealType == null || m.Type == mealType));

            if (mealToDelete == null) return new ResultInfo { Message = $"{recipe} is not planned for {date.ToShortDateString()}", Success = false };

            await _repo.DeleteMeal(mealToDelete);
            return new ResultInfo { Message = $"Deleted {mealToDelete.Recipe} from {mealToDelete.Date.ToShortDateString()}", Success = true };
        }

        public async Task<ResultInfo> ExportRawMealsToFile(string filePath, DateTime? startDate, DateTime? endDate)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) return new ResultInfo { Message = $"{filePath} directory doesn't exist", Success = false };
            if (!filePath.ToLower().EndsWith(".json")) return new ResultInfo { Message = $"{filePath} is not a json", Success = false };

            var meals = await _repo.GetMeals(startDate, endDate);
            var json = JsonConvert.SerializeObject(meals, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, json);

            return new ResultInfo { Message = $"Created {filePath}", Success = true };
        }

        public async Task<ResultInfo> ExportFullMealsToFile(string filePath, DateTime? startDate, DateTime? endDate)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) return new ResultInfo { Message = $"{filePath} directory doesn't exist", Success = false };
            if (!filePath.ToLower().EndsWith(".json")) return new ResultInfo { Message = $"{filePath} is not a json", Success = false };

            var meals = await GetMeals(startDate, endDate);
            var json = JsonConvert.SerializeObject(meals, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, json);

            return new ResultInfo { Message = $"Created {filePath}", Success = true };
        }

        public async Task<ResultInfo> LoadRawMealsFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return new ResultInfo { Message = $"{filePath} does not exist", Success = false };
            if (!filePath.ToLower().EndsWith(".json")) return new ResultInfo { Message = $"{filePath} is not a json", Success = false };

            var fileContents = await File.ReadAllTextAsync(filePath);
            if (string.IsNullOrEmpty(fileContents)) return new ResultInfo { Message = $"{filePath} is empty", Success = false };

            var meals = JsonConvert.DeserializeObject<List<RawMealInfo>>(fileContents);
            if (meals == null) return new ResultInfo { Message = $"{filePath} cannot be deserialized", Success = false };

            var recipeList = meals.Select(m => m.Recipe).ToList();
            var ingredientList = meals.SelectMany(m => m.Changes).Select(c => c.Ingredient.Name).ToList();

            var recipes = await _repo.GetRecipes();
            var ingredients = await _repo.GetIngredients(ingredientList.ToArray());

            recipeList.RemoveAll(r => recipes.Contains(r));
            ingredientList.RemoveAll(i => ingredients.Any(ir => ir.Name == i));
            var message = string.Empty;
            if (recipeList.Count > 0) message = $"The following recipes are missing from reference: {string.Join(", ", recipeList)}.";
            if (ingredientList.Count > 0) message += $" The following ingredients are missing from reference: {string.Join(", ", ingredientList)}.";
            if (!string.IsNullOrEmpty(message)) return new ResultInfo { Message = message.Trim(), Success = false };

            await _repo.SaveMeals(meals.ToArray());
            return new ResultInfo { Message = $"Loaded {filePath}", Success = true };
        }

        public async Task<FullMealInfo[]> GetMeals(DateTime? startDate, DateTime? endDate)
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
