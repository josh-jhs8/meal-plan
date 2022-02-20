using Autofac;
using System.Reflection;

namespace MealPlan
{
    internal class ControllerRegister
    {
        public static void RegisterControllers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Controller"))
                .InstancePerDependency();
        }
    }
}
