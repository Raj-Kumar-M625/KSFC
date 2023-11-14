CREATE TABLE [dbo].[ExecAppImei]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[IMEINumber] NVARCHAR(50) NOT NULL,
	[Comment] NVARCHAR(100),
	[EffectiveDate] DATE NOT NULL,
	[ExpiryDate] DATE NOT NULL,
	[IsSupportPerson] BIT NOT NULL DEFAULT 0,
	[EnableLog] BIT NOT NULL DEFAULT 0,
	[ExecAppDetailLevel] INT NOT NULL DEFAULT 2
)
