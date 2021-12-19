CREATE TABLE [dbo].[CharacterSequenceProperty]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ActionId] INT NOT NULL, 
    [CharacterSequence] VARCHAR(255) NOT NULL, 
    CONSTRAINT [FK_CharacterSequenceProperty_ToTable] FOREIGN KEY (ActionId) REFERENCES Action(Id)
    ON DELETE CASCADE
)
