CREATE TABLE [dbo].[MealChange]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[MealId] INT NOT NULL,
	[IngredientReferenceId] INT NOT NULL,
	[AddOrRemove] VARCHAR(6) NOT NULL,
	[Amount] DECIMAL NOT NULL,
	[ParentMealChangeId] INT NULL,
	CONSTRAINT [MealChange_PRIMARY_KEY_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [MealChange_UNIQUE_MealId_IngredientReferenceId_AddOrRemove] UNIQUE NONCLUSTERED ([MealId] ASC, [IngredientReferenceId] ASC, [AddOrRemove] ASC),
	CONSTRAINT [MealChange_FOREIGN_KEY_MealId] FOREIGN KEY ([MealId]) REFERENCES [Meal]([Id]),
	CONSTRAINT [MealChange_FOREIGN_KEY_IngredientReferenceId] FOREIGN KEY ([IngredientReferenceId]) REFERENCES [IngredientReference]([Id]),
	CONSTRAINT [MealChange_FOREIGN_KEY_ParentMealChangeId] FOREIGN KEY ([ParentMealChangeId]) REFERENCES [MealChange]([Id]),
	CONSTRAINT [MealChange_CHECK_AddOrRemove] CHECK ([AddOrRemove] IN ('ADD', 'REMOVE'))
)
