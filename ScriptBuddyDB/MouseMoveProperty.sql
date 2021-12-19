CREATE TABLE [dbo].[MouseMoveProperty]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ActionId] INT NOT NULL,
    [XPosition] INT NOT NULL,
    [YPosition] INT NOT NULL, 
    CONSTRAINT [FK_MouseMoveProperty_Action] FOREIGN KEY (ActionId) REFERENCES Action(Id)
    ON DELETE CASCADE
)
