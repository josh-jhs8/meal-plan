using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan
{
    internal class MealController : IConsoleController
    {
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
        }

        private void Export(string[] args)
        {
            //Requires file path
            //Optional date range???
        }

        private void List(string[] args) 
        {
            //Optional date range???
        }

        private void View(string[] args)
        {
            //For specific date
            var dateStr = ConsoleUtil.GetFirstParameter(args);
            if (!DateTime.TryParse(dateStr, out var date))
            {
                Console.WriteLine($"{dateStr} is not a valid date in dd.MM.yyyy format");
                return;
            }


        }

        private void Add(string[] args)
        {

        }

        private void Delete(string[] args)
        {
            //Requires date and recipe name
        }

        
    }
}
