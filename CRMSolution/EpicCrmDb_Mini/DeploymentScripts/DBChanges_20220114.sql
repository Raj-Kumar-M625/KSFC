ALTER TABLE [dbo].[CodeTable]
ALTER COLUMN [CodeValue] NVARCHAR(150) NOT NULL
GO

INSERT INTO "CodeTable" ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive", "TenantId")
VALUES ('Notification', '2022-01-14 06:50:00.000000', 'Server will be down from 10:00pm to 11:00pm on 16-01-2022, Start Day, EndDay or Upload service might not work', 10, 'True', 1);