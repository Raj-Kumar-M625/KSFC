CREATE INDEX [IX_SalesPerson_StaffCode]
	ON [dbo].[SalesPerson]
	(StaffCode)
	INCLUDE ([Phone])
