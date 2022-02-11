// See https://aka.ms/new-console-template for more information
using Autofac;
using MealPlan;
using MealPlan.DependencyInjection;

Console.WriteLine("Hello, World!");
Console.WriteLine(String.Join(",", args) + args.Length.ToString());

if (args.Count(a => a.StartsWith("-")) != 1)
{
    Console.WriteLine("A single type flag must be supplied.");
    return;
}

var containerBuilder = DependencyInjector.CreateContainerBuilder();
ControllerRegister.RegisterControllers(containerBuilder);
var container = containerBuilder.Build();

if (args.Contains(MealPlanConsts.ShortIngredientFlag) || args.Contains(MealPlanConsts.LongIngredientFlag))
{
    using (var scope = container.BeginLifetimeScope()) 
    {
        var controller = scope.Resolve<IngredientController>();
        controller.HandleAction(args);
    }
}
else if (args.Contains(MealPlanConsts.ShortRecipeFlag) || args.Contains(MealPlanConsts.LongRecipeFlag))
{
    using (var scope = container.BeginLifetimeScope()) 
    {
        var controller = scope.Resolve<RecipeController>();
        controller.HandleAction(args);
    }
}
else
{
    Console.WriteLine("Failed to recognise supplied type flag");
}