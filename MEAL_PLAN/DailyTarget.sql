﻿CREATE TABLE [dbo].[DailyTarget]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[StartDate] DATE NOT NULL,
	[Calories] INT NOT NULL,
	[Protein] DECIMAL NOT NULL,
	[Carbs] DECIMAL NOT NULL,
	[Fats] DECIMAL NOT NULL,
	CONSTRAINT [DailyTarget_PRIMARY_KEY_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [DailyTarget_UNIQUE_StartDate] UNIQUE NONCLUSTERED ([StartDate] ASC)
)
