using MealPlan.Repo.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan.Repo.Dto.Repos
{
    public interface IIngredientLogicRepo
    {
        Task<IngredientInfo[]> GetIngredientReferences();

        Task<IngredientInfo?> GetIngredientReference(string name);

        Task SaveIngredients(IngredientInfo[] ingredients);

        Task DeleteIngredient(string name);
    }
}
