﻿using MealPlan.Logic.DomainObject.Logics;
using MealPlan.Logic.DomainObject.Models;
using MealPlan.Repo.Dto.Models;
using MealPlan.Repo.Dto.Repos;
using Newtonsoft.Json;

namespace MealPlan.Logic
{
    public class RecipeLogic : IRecipeLogic
    {
        private readonly IRecipeLogicRepo _repo;

        public RecipeLogic(IRecipeLogicRepo repo)
        {
            _repo = repo;
        }

        public async Task<ResultInfo> AddRecipe(RecipeInfo recipeInfo)
        {
            var existingRecipe = await _repo.GetRecipe(recipeInfo.Name);
            if (existingRecipe != null) return new ResultInfo { Message = $"{recipeInfo.Name} already exists in Recipes", Success = false };

            await _repo.SaveRecipes(new RecipeInfo[] { recipeInfo });
            return new ResultInfo { Message = $"Added {recipeInfo.Name}", Success = true };
        }

        public async Task<ResultInfo> DeleteRecipe(string name)
        {
            var existingRecipe = await _repo.GetRecipe(name);
            if (existingRecipe == null) return new ResultInfo { Message = $"{name} doesn't exist in Recipes", Success = false };

            await _repo.DeleteRecipe(name);
            return new ResultInfo { Message = $"Deleted {name}", Success = true};
        }

        public async Task<ResultInfo> ExportRecipesToFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Directory == null || !fileInfo.Directory.Exists) return new ResultInfo { Message = $"{filePath} directory doesn't exist", Success = false };
            if (!filePath.ToLower().EndsWith(".json")) return new ResultInfo { Message = $"{filePath} is not a json", Success = false };

            var recipes = await _repo.GetRecipes();
            var json = JsonConvert.SerializeObject(recipes, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, json);

            return new ResultInfo { Message = $"Created {filePath}", Success = true };
        }

        public async Task<ResultInfo> LoadRecipesFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return new ResultInfo { Message = $"{filePath} does not exist", Success = false };
            if (!filePath.ToLower().EndsWith(".json")) return new ResultInfo { Message = $"{filePath} is not a json", Success = false };

            var fileContents = await File.ReadAllTextAsync(filePath);
            if (string.IsNullOrEmpty(fileContents)) return new ResultInfo { Message = $"{filePath} is empty", Success = false };

            var recipes = JsonConvert.DeserializeObject<List<RecipeInfo>>(fileContents);
            if (recipes == null) return new ResultInfo { Message = $"{filePath} cannot be deserialized", Success = false };

            await _repo.SaveRecipes(recipes.ToArray());

            return new ResultInfo { Message = $"Loaded {filePath}", Success = true };
        }
    }
}
