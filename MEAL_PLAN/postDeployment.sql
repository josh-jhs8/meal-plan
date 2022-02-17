/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DECLARE @MealType_Base TABLE ([Id] INT, [Name] VARCHAR(1000))

INSERT INTO @MealType_Base ([Id], [Name])
VALUES
    (1, 'Breakfast'),
    (2, 'Lunch'),
    (3, 'Dinner'),
    (4, 'Snacks')

MERGE INTO [MealType] a
USING @MealType_Base b
    ON a.[Id] = b.[Id]
WHEN MATCHED THEN
    UPDATE SET [Name] = b.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Id], [Name])
    VALUES (b.[Id], b.[Name])
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;