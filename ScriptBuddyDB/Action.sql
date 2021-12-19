CREATE TABLE [dbo].[Action]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ScriptId] INT NOT NULL, 
    [ActionTypeId] INT NOT NULL, 
    [ActionPosition] INT NOT NULL, 
    CONSTRAINT [FK_Action_Script] FOREIGN KEY (ScriptId) REFERENCES Script(Id)
    ON DELETE CASCADE
)
