CREATE TABLE [dbo].[KeyListenerProperty]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ActionId] INT NOT NULL, 
    [Key] VARCHAR(255) NOT NULL, 
    [ActionEndPosition] INT NOT NULL, 
    CONSTRAINT [FK_KeyListenerProperty_ToTable] FOREIGN KEY (ActionId) REFERENCES Action(Id)
    ON DELETE CASCADE
)
