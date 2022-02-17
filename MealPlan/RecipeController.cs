using MealPlan.Logic.DomainObject.Logics;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;

namespace MealPlan
{
    internal class RecipeController
    {
        private readonly IRecipeControllerRepo _repo;
        private readonly IRecipeLogic _logic;

        public RecipeController(IRecipeControllerRepo repo, IRecipeLogic logic)
        {
            _repo = repo;
            _logic = logic;
        }

        internal void HandleAction(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(string.Join(" ", args) + " is an invalid argument set");
                return;
            }

            var action = args[0];
            if (action == MealPlanConsts.LoadAction) Load(args);
            else if (action == MealPlanConsts.ListAction) List(args);
            else if (action == MealPlanConsts.ViewAction) View(args);
            else if (action == MealPlanConsts.DeleteAction) Delete(args);
            else if (action == MealPlanConsts.AddAction) Add(args);
            else if (action == MealPlanConsts.ExportAction) Export(args);
            else Console.WriteLine($"{action} not a recognised command");

            return;
        }

        private void Load(string[] args)
        {
            var filePath = GetFirstParameter(args);
            if (filePath == null)
            {
                Console.WriteLine("A file path must be supplied");
                return;
            }

            var result = _logic.LoadRecipesFromFile(filePath).Result;
            Console.WriteLine(result.Message);
        }

        private void Export(string[] args)
        {
            var filePath = GetFirstParameter(args);
            if (filePath == null)
            {
                Console.WriteLine("A file path must be supplied");
                return;
            }

            var result = _logic.ExportRecipesToFile(filePath).Result;
            Console.WriteLine(result.Message);
        }

        private void Add(string[] args)
        {
            Console.WriteLine("Enter a recipe name:");
            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("A Recipe name must be supplied");
                return;
            }

            var existingRecipe = _repo.GetRecipe(name).Result;
            if (existingRecipe != null)
            {
                Console.WriteLine($"{name} already exists in Recipes");
                return;
            }

            var recipeIngredients = new List<RecipeIngredientInfo>();
            Console.WriteLine("Add new ingredient or press enter to finish ingredients list:");
            var ingredientName = Console.ReadLine();
            while (!string.IsNullOrEmpty(ingredientName))
            {
                Console.WriteLine("How much of this ingredient:");
                var amount = Console.ReadLine();
                if (!decimal.TryParse(amount, out var ingredientAmount))
                {
                    Console.WriteLine("Amount should be a number");
                }
                recipeIngredients.Add(new RecipeIngredientInfo { Name = ingredientName, Amount = ingredientAmount });

                Console.WriteLine("Add new ingredient or press enter to finish ingredients list:");
                ingredientName = Console.ReadLine();
            }

            var recipeSteps = new List<string>();
            Console.WriteLine("Add step to instructions:");
            var step = Console.ReadLine();
            while (!string.IsNullOrEmpty(step))
            {
                recipeSteps.Add(step);

                Console.WriteLine("Add step to instructions:");
                step = Console.ReadLine();
            }

            var recipe = new RecipeInfo
            {
                Name = name,
                Ingredients = recipeIngredients.ToArray(),
                Steps = recipeSteps.ToArray()
            };

            var result = _logic.AddRecipe(recipe).Result;
            Console.WriteLine(result.Message);
        }

        private void Delete(string[] args)
        {
            var name = GetFirstParameter(args);
            if (name == null)
            {
                Console.WriteLine("A Recipe name must be supplied");
                return;
            }

            var result = _logic.DeleteRecipe(name).Result;
            Console.WriteLine(result.Message);
        }

        private void List(string[] args)
        {
            var recipes = _repo.GetRecipes().Result;
            foreach (var recipe in recipes)
            {
                var macros = _logic.CalculateMacrosForRecipe(recipe.Name).Result;
                Console.WriteLine($"{recipe.Name} ({macros.Calories} kcal - C: {macros.Carbs}g, F: {macros.Fats}g, P: {macros.Protein}g)");
            }
        }

        private void View(string[] args)
        {
            var name = GetFirstParameter(args);
            if (name == null)
            {
                Console.WriteLine("A Recipe name must be supplied");
                return;
            }

            var recipe = _repo.GetRecipe(name).Result;

            if (recipe == null)
            {
                Console.WriteLine($"{name} does not exist in Recipes");
                return;
            }

            var macros = _logic.CalculateMacrosForRecipe(recipe.Name).Result;

            Console.WriteLine($"### {recipe.Name} ###");

            Console.WriteLine("+ Macros");
            Console.WriteLine($"  - Calories: {macros.Calories} kcal");
            Console.WriteLine($"  - Protein: {macros.Protein} g");
            Console.WriteLine($"  - Carbs: {macros.Carbs} g");
            Console.WriteLine($"  - Fats: {macros.Fats} g");

            Console.WriteLine("+ Ingredients:");
            foreach (var ingredient in recipe.Ingredients) Console.WriteLine($"  - {ingredient.Name}: {ingredient.Amount} {ingredient.UnitName}");

            Console.WriteLine("+ Instructions:");
            for (var i = 0; i < recipe.Steps.Length; i++) Console.WriteLine($"  {i + 1}. {recipe.Steps[i]}");
        }

        private string? GetFirstParameter(string[] args)
        {
            string firstParam = null;
            for (var i = 1; i < args.Length; i++)
            {
                if (args[i] == MealPlanConsts.ShortRecipeFlag || args[i] == MealPlanConsts.LongRecipeFlag) continue;
                else
                {
                    firstParam = args[i];
                    break;
                }
            }
            return firstParam;
        }
    }
}
