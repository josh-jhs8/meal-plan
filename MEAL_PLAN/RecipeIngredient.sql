CREATE TABLE [dbo].[RecipeIngredient]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[RecipeId] INT NOT NULL,
	[IngredientReferenceId] INT NOT NULL,
	[Amount] DECIMAL(10,4) NOT NULL,
	CONSTRAINT [RecipeIngredient_PRIMARY_KEY_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [RecipeIngredient_UNIQUE_RecipeId_IngredientReferenceId] UNIQUE NONCLUSTERED ([RecipeId] ASC, [IngredientReferenceId] ASC),
	CONSTRAINT [RecipeIngredient_FOREIGN_KEY_RecipeId] FOREIGN KEY ([RecipeId]) REFERENCES [Recipe]([Id]),
	CONSTRAINT [RecipeIngredient_FOREIGN_KEY_IngredientReferenceId] FOREIGN KEY ([IngredientReferenceId]) REFERENCES [IngredientReference]([Id])
)
