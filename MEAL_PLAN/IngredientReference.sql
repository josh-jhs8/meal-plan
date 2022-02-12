﻿CREATE TABLE [dbo].[IngredientReference]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[Name] VARCHAR(1000) NOT NULL,
	[PerAmount] DECIMAL(10,2) NOT NULL,
	[UnitName] VARCHAR(255) NOT NULL,
	[Protein] DECIMAL(10,2) NOT NULL,
	[Fat] DECIMAL(10,2) NOT NULL,
	[Carbs] DECIMAL(10,2) NOT NULL,
	[Calories] INT NOT NULL,
	CONSTRAINT [IngredientReference_PRIMARY_KEY_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [IngredientReference_UNIQUE_Name] UNIQUE NONCLUSTERED ([Name] ASC)
)