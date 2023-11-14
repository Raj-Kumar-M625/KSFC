-- Jan 10 2020

-- Perf issue on Multiplex site
CREATE NONCLUSTERED INDEX [IX_SqliteActionBatch_ExpenseId] 
ON [dbo].[SqliteActionBatch]
(
	[ExpenseId] ASC
)
INCLUDE ( EmployeeId, TotalExpenseAmount, Id, expenseDate)
 WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
