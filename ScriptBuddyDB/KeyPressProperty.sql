CREATE TABLE [dbo].[KeyPressProperty]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ActionId] INT NOT NULL, 
    [KeyPressed] VARCHAR(255) NOT NULL, 
    [PressType] VARCHAR(255) NOT NULL, 
    CONSTRAINT [FK_KeyPressProperty_Action] FOREIGN KEY (ActionId) REFERENCES Action(Id)
    ON DELETE CASCADE
)
