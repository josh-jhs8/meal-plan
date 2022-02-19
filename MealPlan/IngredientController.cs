using MealPlan.Logic.DomainObject.Logics;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;

namespace MealPlan
{
    internal class IngredientController : IConsoleController
    {
        private readonly IIngredientControllerRepo _repo;
        private readonly IIngredientLogic _logic;

        public IngredientController(IIngredientControllerRepo repo, IIngredientLogic logic)
        {
            _repo = repo;
            _logic = logic;
        }

        public void HandleAction(string[] args)
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
            var filePath = ConsoleUtil.GetFirstParameter(args);
            if (filePath == null)
            {
                Console.WriteLine("A file path must be supplied");
                return;
            }

            var result = _logic.LoadIngredientsFromFile(filePath).Result;
            Console.WriteLine(result.Message);
        }

        private void Export(string[] args)
        {
            var filePath = ConsoleUtil.GetFirstParameter(args);
            if (filePath == null)
            {
                Console.WriteLine("A file path must be supplied");
                return;
            }

            var result = _logic.ExportIngredientsToFile(filePath).Result;
            Console.WriteLine(result.Message);
        }

        private void Add(string[] args)
        {
            Console.WriteLine("Enter an ingredient name:");
            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("An ingredient name must be supplied");
                return;
            }

            var existingIngredient = _repo.GetIngredientReference(name).Result;
            if (existingIngredient != null)
            {
                Console.WriteLine($"{name} already exists in ingredient references");
                return;
            }

            Console.WriteLine("Enter the Unit Name:");
            var unitName = Console.ReadLine();
            if (string.IsNullOrEmpty(unitName))
            {
                Console.WriteLine("A unit name must be entered");
                return;
            }

            Console.WriteLine("Enter the Units per reference value:");
            var perAmountStr = Console.ReadLine();
            if (string.IsNullOrEmpty(perAmountStr) || !decimal.TryParse(perAmountStr, out var perAmount))
            {
                Console.WriteLine("Units per reference value must be supplied and must be a number");
                return;
            }

            Console.WriteLine("Enter the number of Calories (kcal):");
            var caloriesStr = Console.ReadLine();
            if (string.IsNullOrEmpty(caloriesStr) || !int.TryParse(caloriesStr, out var calories))
            {
                Console.WriteLine("Calories must be supplied and must be an interger");
                return;
            }

            Console.WriteLine("Enter the amount of Carbs (g):");
            var carbsStr = Console.ReadLine();
            if (string.IsNullOrEmpty(carbsStr) || !decimal.TryParse (carbsStr, out var carbs))
            {
                Console.WriteLine("Carbs must be supplied and must be a number");
                return;
            }

            Console.WriteLine("Enter the amount of Fats (g):");
            var fatsStr = Console.ReadLine();
            if (string.IsNullOrEmpty(fatsStr) || !decimal.TryParse(fatsStr, out var fats))
            {
                Console.WriteLine("Fats must be supplied and must be a number");
                return;
            }

            Console.WriteLine("Enter the amount of Protein (g):");
            var proteinStr = Console.ReadLine();
            if (string.IsNullOrEmpty(proteinStr) || !decimal.TryParse(proteinStr, out var protein))
            {
                Console.WriteLine("Protein must be supplied and must be a number");
                return;
            }

            var ingredient = new IngredientInfo
            {
                Name = name,
                PerAmount = perAmount,
                UnitName = unitName,
                Calories = calories,
                Protein = protein,
                Fats = fats,
                Carbs = carbs
            };

            var result = _logic.AddIngredient(ingredient).Result;
            Console.WriteLine(result.Message);
        }

        private void Delete(string[] args)
        {
            var name = ConsoleUtil.GetFirstParameter(args);
            if (name == null)
            {
                Console.WriteLine("An ingredient name must be supplied");
                return;
            }

            var result = _logic.DeleteIngredient(name).Result;
            Console.WriteLine(result.Message);
        }

        private void List(string[] args)
        {
            var ingredients = _repo.GetIngredientReferences().Result;
            foreach (var ingredient in ingredients)
            {
                Console.WriteLine(ingredient.Name);
            }
        }

        private void View(string[] args)
        {
            var name = ConsoleUtil.GetFirstParameter(args);
            if (name == null)
            {
                Console.WriteLine("A Recipe name must be supplied");
                return;
            }

            var ingredient = _repo.GetIngredientReference(name).Result;
            if (ingredient == null)
            {
                Console.WriteLine($"{name} does not exist in ingredient references");
                return;
            }

            Console.WriteLine($"### {ingredient.Name} ###");
            Console.WriteLine($"Values per {ingredient.PerAmount} {ingredient.UnitName}");
            Console.WriteLine($"+ Calories: {ingredient.Calories} kcal");
            Console.WriteLine($"+ Carbs: {ingredient.Carbs} g");
            Console.WriteLine($"+ Fats: {ingredient.Fats} g");
            Console.WriteLine($"+ Protein: {ingredient.Protein} g");
        }
    }
}
