Alter table BankStatements
Add IsProcessed bit null,
IsDuplicate bit null,
IsJunk bit null,
IsSuccess bit null


Update BankStatements
Set IsDuplicate=0,IsProcessed=0,
IsJunk=0,IsSuccess=0