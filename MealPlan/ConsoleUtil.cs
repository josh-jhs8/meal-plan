﻿namespace MealPlan
{
    internal static class ConsoleUtil
    {
        internal static string? GetFirstParameter(string[] args)
        {
            string firstParam = null;
            for (var i = 1; i < args.Length; i++)
            {
                if (MealPlanConsts.Flags.Contains(args[i])) continue;
                else
                {
                    firstParam = args[i];
                    break;
                }
            }
            return firstParam;
        }
    }
}
