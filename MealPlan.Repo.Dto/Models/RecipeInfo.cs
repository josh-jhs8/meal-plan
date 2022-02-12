namespace MealPlan.Repo.Dto.Models
{
    public class RecipeInfo
    {
        public string Name { get; set; }
        public RecipeIngredientInfo[] Ingredients { get; set; }
        public string[] Steps { get; set; }
    }
}
