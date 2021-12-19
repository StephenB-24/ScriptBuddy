CREATE TABLE [dbo].[Script]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [Name] VARCHAR(255) NOT NULL, 
    [Description] VARCHAR(255) NOT NULL, 
    [Accessibility] BIT NOT NULL, 
    [CommunityTagId] INT NOT NULL, 
    [TimeLastSaved] DATETIME NOT NULL
)