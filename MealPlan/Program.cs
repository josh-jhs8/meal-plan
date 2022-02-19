// See https://aka.ms/new-console-template for more information
using Autofac;
using MealPlan;
using MealPlan.DependencyInjection;

if (args.Count(a => a.StartsWith("-")) != 1)
{
    Console.WriteLine("A single type flag must be supplied.");
    return;
}

var containerBuilder = DependencyInjector.CreateContainerBuilder();
ControllerRegister.RegisterControllers(containerBuilder);
var container = containerBuilder.Build();

using (var scope = container.BeginLifetimeScope())
{
    var controller = GetController(scope, args);
    controller.HandleAction(args);
}

static IConsoleController GetController(ILifetimeScope scope, string[] args)
{
    if (args.Contains(MealPlanConsts.ShortIngredientFlag) || args.Contains(MealPlanConsts.LongIngredientFlag))
        return scope.Resolve<IngredientController>();
    if (args.Contains(MealPlanConsts.ShortRecipeFlag) || args.Contains(MealPlanConsts.LongRecipeFlag))
        return scope.Resolve<RecipeController>();
    if (args.Contains(MealPlanConsts.ShortMealFlag) || args.Contains(MealPlanConsts.LongMealFlag))
        return scope.Resolve<MealController>();

    throw new Exception("Invalid type flag supplied");
    
}