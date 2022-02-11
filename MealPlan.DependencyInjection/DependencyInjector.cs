using Autofac;
using MealPlan.Db.Data;
using MealPlan.Logic;
using MealPlan.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan.DependencyInjection
{
    public static class DependencyInjector
    {
        public static ContainerBuilder CreateContainerBuilder()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(LogicAssembly)))
                .Where(t => t.Name.EndsWith("Logic"))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(RepoAssembly)))
                .Where(t => t.Name.EndsWith("Repo"))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterType<MealPlanDb>();

            return containerBuilder;
        }

    }
}
