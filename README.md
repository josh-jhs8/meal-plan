# meal-plan
Console app for Meal Planning
Anywhere a date is used i'll stick to dd.MM.yyyy format

## Ingredients

### Add
```dotnet MealPlan.dll add (-i|-ingredient)```

This launches an interactive prompt to add a new ingredient

### View
```dotnet MealPlan.dll view (-i|-ingredient) <name>```

This allows the user to view the reference information for a specific ingredient.
Name is a required argument.

### List
```dotnet MealPlan.dll list (-i|-ingredient)```

### Delete
```dotnet MealPlan.dll delete (-i|-ingredient) <name>```

### Export
```dotnet MealPlan.dll export (-i|-ingredient) <file>```

### Load
```dotnet MealPlan.dll load (-i|-ingredient) <file>```

## Recipes

### Add
```dotnet MealPlan.dll add (-r|-recipe)```

### View
```dotnet MealPlan.dll view (-r|-recipe) <name>```

### List
```dotnet MealPlan.dll list (-r|-recipe)```

### Delete
```dotnet MealPlan.dll delete (-r|-recipe) <name>```

### Export
```dotnet MealPlan.dll export (-r|-recipe) <file>```

### Load
```dotnet MealPlan.dll load (-r|-recipe) <file>```