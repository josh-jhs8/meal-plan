namespace MealPlan.Repo.Dto.Models
{
    public class IngredientInfo
    {
        public string Name { get; set; }
        public decimal PerAmount { get; set; }
        public string UnitName { get; set; }
        public decimal Carbs { get; set; }
        public decimal Protein { get; set; }
        public decimal Fats { get; set; }
        public int Calories { get; set; }
    }
}
