CREATE TABLE [dbo].[MouseClickProperty]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ActionId] INT NOT NULL, 
    [Button] VARCHAR(30) NOT NULL, 
    [ClickType] VARCHAR(5) NOT NULL, 
    CONSTRAINT [FK_MouseClickProperty_Action] FOREIGN KEY (ActionId) REFERENCES Action(Id)
)
