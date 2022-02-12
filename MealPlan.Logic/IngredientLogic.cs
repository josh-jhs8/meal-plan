using MealPlan.Logic.DomainObject.Logics;
using MealPlan.Logic.DomainObject.Models;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;

namespace MealPlan.Logic
{
    public class IngredientLogic : IIngredientLogic
    {
        private readonly IIngredientLogicRepo _repo;

        public IngredientLogic(IIngredientLogicRepo repo)
        {
            _repo = repo;
        }

        public async Task<ResultInfo> AddIngredient(IngredientInfo ingredient)
        {
            var existingIngredient = await _repo.GetIngredientReference(ingredient.Name);
            if (existingIngredient != null) return new ResultInfo { Message = $"{ingredient.Name} already exists in Ingredient References", Success = false };

            await _repo.SaveIngredients(new IngredientInfo[] { ingredient });
            return new ResultInfo { Message = $"Added {ingredient.Name}", Success = true };
        }

        public async Task<ResultInfo> DeleteIngredient(string name)
        {
            var existingIngredient = await _repo.GetIngredientReference(name);
            if (existingIngredient == null) return new ResultInfo { Message = $"{name} does not exist in Ingredient References", Success = false };

            await _repo.DeleteIngredient(name);
            return new ResultInfo { Message = $"Deleted {name}", Success = true };
        }

        public async Task<ResultInfo> ExportIngredientsToFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) return new ResultInfo { Message = $"{filePath} directory doesn't exist", Success = false };
            if (!filePath.ToLower().EndsWith(".csv")) return new ResultInfo { Message = $"{filePath} is not a csv", Success = false };

            var ingredients = await _repo.GetIngredientReferences();

            var output = new List<string>();
            output.Add(GetIngredientsFileHeader());
            output.AddRange(ingredients.Select(i => MapFileRowToIngredient(i)));

            await File.WriteAllLinesAsync(filePath, output);

            return new ResultInfo { Message = $"Created {filePath}", Success = true };
        }

        public async Task<ResultInfo> LoadIngredientsFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return new ResultInfo { Message = $"{filePath} does not exist", Success = false };
            if (!filePath.ToLower().EndsWith(".csv")) return new ResultInfo { Message = $"{filePath} is not a csv", Success = false };

            var fileContents = await File.ReadAllLinesAsync(filePath);
            if (fileContents.Length == 0) return new ResultInfo { Message = $"{filePath} is empty", Success = false };

            var expectedHeader = GetIngredientsFileHeader();
            var expectedLength = expectedHeader.Split(",").Length;
            if (fileContents[0] != expectedHeader) 
            { 
                return new ResultInfo { Message = $"{filePath} doesn't include all required columns", Success = false }; 
            }
            if (fileContents.Any(l => l.Split(",").Length != expectedLength))
            {
                return new ResultInfo { Message = $"Not all rows in {filePath} have the required columns", Success = false };
            }

            var ingredients = fileContents.Skip(1).Select(l => MapFileRowToIngredient(l.Split(","))).ToArray();
            await _repo.SaveIngredients(ingredients);

            return new ResultInfo { Message = $"Loaded {filePath}", Success = true };
        }

        private string GetIngredientsFileHeader()
        {
            return string.Join(",", new string[] 
            { 
                "Name",
                "Per Amount",
                "Units",
                "Calories (kcal)",
                "Carbs (g)",
                "Fats (g)",
                "Protein (g)"
            });
        }

        private string MapFileRowToIngredient(IngredientInfo ingredient)
        {
            return string.Join(",", new string[]
            {
                ingredient.Name,
                ingredient.PerAmount.ToString(),
                ingredient.UnitName,
                ingredient.Calories.ToString(),
                ingredient.Carbs.ToString(),
                ingredient.Fats.ToString(),
                ingredient.Protein.ToString()
            });
        }

        private IngredientInfo MapFileRowToIngredient(string[] rowElements)
        {
            return new IngredientInfo
            {
                Name = rowElements[0].ToLower(),
                PerAmount = Convert.ToDecimal(rowElements[1]),
                UnitName = rowElements[2],
                Calories = Convert.ToInt32(rowElements[3]),
                Carbs = Convert.ToDecimal(rowElements[4]),
                Fats = Convert.ToDecimal(rowElements[5]),
                Protein = Convert.ToDecimal(rowElements[6])
            };
        }
    }
}
