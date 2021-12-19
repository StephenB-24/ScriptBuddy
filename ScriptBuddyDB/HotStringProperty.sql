CREATE TABLE [dbo].[HotStringProperty]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ActionId] INT NOT NULL, 
    [InputString] VARCHAR(255) NOT NULL, 
    [OutputString] VARCHAR(255) NOT NULL, 
    CONSTRAINT [FK_HotStringProperty_ToTable] FOREIGN KEY (ActionId) REFERENCES Action(Id)
    ON DELETE CASCADE
)
