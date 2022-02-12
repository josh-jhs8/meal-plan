using Microsoft.EntityFrameworkCore;

namespace MealPlan.Db.Data
{
    public partial class MealPlanDb : DbContext
    {
        public MealPlanDb(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MEAL_PLAN;Integrated Security=true");
        }
    }
}
