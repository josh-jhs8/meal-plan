using MealPlan.Logic.DomainObject.Logics;
using MealPlan.Logic.DomainObject.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan.Controllers
{
    internal class MealController : IConsoleController
    {
        private readonly IMealLogic _logic;

        public MealController(IMealLogic logic)
        {
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
            //Requires file path
            var filePath = ConsoleUtil.GetFirstParameter(args);
            if (filePath == null)
            {
                Console.WriteLine("A file path must be supplied");
                return;
            }

            var result = _logic.LoadRawMealsFromFile(filePath).Result;
            Console.WriteLine(result.Message);
        }

        private void Export(string[] args)
        {
            //Requires file path
            //Optional date range???
            //Optional full info
            var parameters = ConsoleUtil.GetNonFlagActionParameters(args);
            if (parameters.Length == 0)
            {
                Console.WriteLine("A file path must be supplied to export to");
                return;
            }

            var filePath = parameters[0];
            var startDateStr = parameters.Length > 1 ? parameters[1] : null;
            var endDateStr = parameters.Length > 2 ? parameters[2] : null;
            var fullDataSet = parameters.Length > 3 && parameters[3] == "F";

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(startDateStr))
            {
                DateTime date;
                var culture = CultureInfo.InvariantCulture;
                var validStartDate = DateTime.TryParseExact(startDateStr, "dd.MM.yyyy", culture, DateTimeStyles.None, out date);
                if (validStartDate) startDate = date;
                else
                {
                    Console.WriteLine("Start date must be in dd.MM.yyyy format");
                    return;
                }
                if (!string.IsNullOrEmpty(endDateStr))
                {
                    var validEndDate = DateTime.TryParseExact(endDateStr, "dd.MM.yyyy", culture, DateTimeStyles.None, out date);
                    if (validEndDate) endDate = date;
                    else
                    {
                        Console.WriteLine("End date must be dd.MM.yyyy format");
                        return;
                    }
                }
            }

            var result = new ResultInfo();
            if (fullDataSet) result = _logic.ExportFullMealsToFile(filePath, startDate, endDate).Result;
            else result = _logic.ExportRawMealsToFile(filePath, startDate, endDate).Result;

            Console.WriteLine(result.Message);
        }

        private void List(string[] args) 
        {
            //Optional date range???
            var parameters = ConsoleUtil.GetNonFlagActionParameters(args);
            var startDateStr = parameters.Length > 0 ? parameters[0] : null;
            var endDateStr = parameters.Length > 1 ? parameters[1] : null;

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(startDateStr))
            {
                DateTime date;
                var culture = CultureInfo.InvariantCulture;
                var validStartDate = DateTime.TryParseExact(startDateStr, "dd.MM.yyyy", culture, DateTimeStyles.None, out date);
                if (validStartDate) startDate = date;
                else
                {
                    Console.WriteLine("Start date must be in dd.MM.yyyy format");
                    return;
                }
                if (!string.IsNullOrEmpty(endDateStr))
                {
                    var validEndDate = DateTime.TryParseExact(endDateStr, "dd.MM.yyyy", culture, DateTimeStyles .None, out date);
                    if (validEndDate) endDate = date;
                    else
                    {
                        Console.WriteLine("End date must be dd.MM.yyyy format");
                        return;
                    }
                }
            }

            var meals = _logic.GetMeals(startDate, endDate).Result;
            if (meals.Length == 0)
            {
                Console.WriteLine($"No meals planned in specified date range");
                return;
            }

            var currentDate = DateTime.MaxValue;
            var dateMacros = new MacroInfo();
            foreach (var meal in meals.OrderByDescending(m => m.Date))
            {
                if (currentDate != meal.Date)
                {
                    if (currentDate != DateTime.MaxValue) Console.WriteLine($"### {currentDate.ToShortDateString()} Totals : {GetMacroString(dateMacros)}");
                    currentDate = meal.Date;
                    dateMacros = new MacroInfo();
                    Console.WriteLine($"### {currentDate.ToShortDateString()} ###");
                }
                dateMacros.Calories += meal.Macros.Calories;
                dateMacros.Protein += meal.Macros.Protein;
                dateMacros.Fats += meal.Macros.Fats;
                dateMacros.Carbs += meal.Macros.Carbs;

                Console.WriteLine($"{meal.Name} - {meal.Type} ({GetMacroString(meal.Macros)})");
            }
            Console.WriteLine($"### {currentDate.ToShortDateString()} Totals : {GetMacroString(dateMacros)}");
        }

        private void View(string[] args)
        {
            //For specific date
            var dateStr = ConsoleUtil.GetFirstParameter(args);
            if (dateStr == null)
            {
                Console.WriteLine("A date parameter in dd.MM.yyyy format is required");
                return;
            }
            var culture = CultureInfo.InvariantCulture;
            if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", culture, DateTimeStyles.None, out var date))
            {
                Console.WriteLine($"{dateStr} is not a valid date in dd.MM.yyyy format");
                return;
            }

            var meals = _logic.GetMeals(date, date).Result;
            if (meals.Length == 0)
            {
                Console.WriteLine($"No meals planned for {dateStr}");
                return;
            }

            foreach (var meal in meals)
            {
                Console.WriteLine($"### {meal.Name} ({meal.Type}) ###");
                Console.WriteLine($"+ Macros: {GetMacroString(meal.Macros)}");
                Console.WriteLine("+ Ingredients: ");
                foreach (var ingredient in meal.Ingredients) Console.WriteLine($"  - {ingredient.Name}: {ingredient.Amount} {ingredient.UnitName}");
            }
        }

        private void Add(string[] args)
        {
            //Interactive
        }

        private void Delete(string[] args)
        {
            //Requires date and recipe name
            //Type is optional
            var parameters = ConsoleUtil.GetNonFlagActionParameters(args);
            if (parameters.Length < 2)
            {
                Console.WriteLine("A date in dd.MM.yyyy format and recipe name are required");
            }

            var dateStr = parameters[0];
            var recipe = parameters[1];
            var mealType = parameters.Length > 2 ? parameters[2] : null;

            var culture = CultureInfo.InvariantCulture;
            if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", culture, DateTimeStyles.None, out var date))
            {
                Console.WriteLine("A date must be supplied in dd.MM.yyyy format");
                return;
            }

            var result = _logic.DeleteMeal(date, recipe, mealType).Result;
            Console.WriteLine(result.Message);
        }

        private string GetMacroString(MacroInfo macros)
        {
            return $"{macros.Calories} kcal, P: {macros.Protein}g, C: {macros.Carbs}g, F: {macros.Fats}g";
        }
    }
}
