CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] VARCHAR(255) NOT NULL, 
    [Password] VARCHAR(255) NOT NULL, 
    [ProfileName] VARCHAR(255) NOT NULL,
)
