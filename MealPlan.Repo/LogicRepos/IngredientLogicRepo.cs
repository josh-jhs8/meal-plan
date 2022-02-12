using MealPlan.Db.Data;
using MealPlan.Repo.ControllerRepos;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using MealPlan.Repo.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan.Repo.LogicRepos
{
    public class IngredientLogicRepo : IngredientControllerRepo, IIngredientLogicRepo
    {
        public IngredientLogicRepo(MealPlanDb db) : base(db)
        {
        }

        public async Task DeleteIngredient(string name)
        {
            var ingredient = await Db.IngredientReferences.FirstAsync(ir => ir.Name == name);
            Db.IngredientReferences.Remove(ingredient);
            await Db.SaveChangesAsync();
        }

        public async Task SaveIngredients(IngredientInfo[] ingredients)
        {
            var existingIngredients = (await Db.IngredientReferences.ToArrayAsync())
                .Where(ir => ingredients.Any(i => ir.Name == i.Name))
                .ToArray();

            foreach (var ingredient in ingredients)
            {
                var existingIngredient = existingIngredients.FirstOrDefault(ei => ei.Name == ingredient.Name);
                if (existingIngredient != null)
                {
                    IngredientMapping.UpdateIngredientReferenceFromIngredientInfo(existingIngredient, ingredient);
                }
                else
                {
                    var ingredientReference = IngredientMapping.MapToIngredientReference(ingredient);
                    await Db.IngredientReferences.AddAsync(ingredientReference);
                }
            }

            await Db.SaveChangesAsync();
        }
    }
}
