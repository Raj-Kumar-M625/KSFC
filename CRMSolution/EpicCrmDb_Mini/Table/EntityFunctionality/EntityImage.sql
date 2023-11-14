CREATE TABLE [dbo].[EntityImage](
	[Id] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[EntityId] [bigint] NOT NULL REFERENCES [dbo].[Entity](Id),
	[ImageId] [bigint] NOT NULL REFERENCES [dbo].[Image](Id),
	[SequenceNumber] [int] NOT NULL
);

