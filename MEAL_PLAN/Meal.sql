CREATE TABLE [dbo].[Meal]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[RecipeId] INT NOT NULL,
	[PlannedDate] DATE NOT NULL,
	[MealTypeId] INT NOT NULL,
	CONSTRAINT [Meal_PRIMARY_KEY_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [Meal_UNIQUE_RecipeId_PlannedDate_MealTypeId] UNIQUE NONCLUSTERED ([RecipeId] ASC, [PlannedDate] ASC, [MealTypeId] ASC),
	CONSTRAINT [Meal_FOREIGN_KEY_RecipeId] FOREIGN KEY ([RecipeId]) REFERENCES [Recipe]([Id]),
	CONSTRAINT [Meal_FOREIGN_KEY_MealTypeId] FOREIGN KEY ([MealTypeId]) REFERENCES [MealType]([Id])
)
