CREATE TABLE [dbo].[Recipe]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[Name] VARCHAR(1000) NOT NULL,
	CONSTRAINT [Recipe_PRIMARY_KEY_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [Recipe_UNIQUE_Name] UNIQUE NONCLUSTERED ([Name] ASC)
)
