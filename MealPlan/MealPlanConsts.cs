namespace MealPlan
{
    internal class MealPlanConsts
    {
        public const string ShortIngredientFlag = "-i";
        public const string LongIngredientFlag = "-ingredient";
        public const string ShortRecipeFlag = "-r";
        public const string LongRecipeFlag = "-recipe";
        public const string ShortMealFlag = "-m";
        public const string LongMealFlag = "-meal";

        public static readonly string[] Flags = new string[] 
        { 
            ShortIngredientFlag, 
            LongIngredientFlag, 
            ShortRecipeFlag, 
            LongRecipeFlag, 
            ShortMealFlag, 
            LongMealFlag 
        };

        public const string ExportAction = "export";
        public const string LoadAction = "load";
        public const string AddAction = "add";
        public const string ListAction = "list";
        public const string ViewAction = "view";
        public const string DeleteAction = "delete";
    }
}
