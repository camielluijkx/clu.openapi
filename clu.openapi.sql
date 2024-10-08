/*

USE [LibraryDB]

SELECT * FROM [__EFMigrationsHistory]

*/


/*

USE [LibraryDB]

DELETE FROM [Books] WHERE [Title] LIKE '%Some new book%'

SELECT * FROM [Authors]
SELECT * FROM [Books]

*/


USE [LibraryDB]

DECLARE @authorId UNIQUEIDENTIFIER = '2902b665-1190-4c70-9915-b9c2d7680450'
DECLARE @bookId UNIQUEIDENTIFIER = '40ff5488-fdab-45b5-bc3a-14302d59869a'

SELECT 
	  a.[Id] AS [AuthorId]
    , a.[FirstName]
	, a.[LastName]
	, b.[Id] AS [BookId]
	, b.[Title]
	, b.[Description]
	, b.[AmountOfPages]
FROM [Authors] a
	INNER JOIN [Books] b
		ON b.[AuthorId] = a.[Id]
--WHERE a.[Id] = @authorId
--	AND b.[Id] = @bookId