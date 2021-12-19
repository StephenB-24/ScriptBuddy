CREATE TABLE [dbo].[MediaKeyProperty]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ActionId] INT NOT NULL, 
    [MediaKey] VARCHAR(45) NOT NULL, 
    CONSTRAINT [FK_MediaKeyProperty_Action] FOREIGN KEY (ActionId) REFERENCES Action(Id)
)