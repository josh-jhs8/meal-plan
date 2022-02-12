using MealPlan.Db.Data;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using MealPlan.Repo.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MealPlan.Repo.ControllerRepos
{
    public class IngredientControllerRepo : IIngredientControllerRepo
    {
        protected readonly MealPlanDb Db;

        public IngredientControllerRepo(MealPlanDb db)
        {
            Db = db;
        }

        public async Task<IngredientInfo?> GetIngredientReference(string name)
        {
            var ingredient = await Db.IngredientReferences.FirstOrDefaultAsync(ir => ir.Name == name);
            if (ingredient == null) return null;

            var mapped = IngredientMapping.MapToIngredientInfo(ingredient);
            return mapped;
        }

        public async Task<IngredientInfo[]> GetIngredientReferences()
        {
            var ingredients = await Db.IngredientReferences.ToArrayAsync();
            var mapped = ingredients.Select(IngredientMapping.MapToIngredientInfo).ToArray();
            return mapped;
        }
    }
}
