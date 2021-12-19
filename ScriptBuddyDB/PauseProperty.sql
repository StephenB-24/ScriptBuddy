CREATE TABLE [dbo].[PauseProperty]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ActionId] INT NOT NULL, 
    [PauseDuration] INT NOT NULL, 
    CONSTRAINT [FK_PauseProperty_Action] FOREIGN KEY (ActionId) REFERENCES Action(Id)
    ON DELETE CASCADE
)