namespace MealPlan.Repo.Dto.Models
{
    public class RawMealInfo
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Recipe { get; set; }
        public MealChangeInfo[] Changes { get; set; }
    }
}
