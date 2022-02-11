CREATE TABLE [dbo].[RecipeIngredient]
(
	[Id] INT NOT NULL,
	[RecipeId] INT NOT NULL,
	[IngredientReferenceId] INT NOT NULL,
	[Amount] DECIMAL(10,4) NOT NULL,
	CONSTRAINT [RecipeIngredient_PRIMARY_KEY_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [RecipeIngredient_UNIQUE_RecipeId_IngredientReferenceId] UNIQUE NONCLUSTERED ([RecipeId] ASC, [IngredientReferenceId] ASC)
)
