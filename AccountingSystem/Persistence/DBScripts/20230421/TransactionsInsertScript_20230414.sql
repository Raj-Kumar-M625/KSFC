select Top 10 *  from  BankTransactions 

select * from Transactions




Insert into CessTransactions
(ReferenceId,VendorId,VendorName,PhoneNumber,AssesmentYear,Amount,ChargeOrPayment,TransactionGeneratedDate,TransactionDate
,ReferenceNumber,ReferenceType,TransactionType,AccountNumber,BankName,UTRNumber,Cheque_No,CreatedBy,CreatedDate)

values
(207,3,'Kia Motors','9998989898','2023-2024','77000.0000000000000000','p','2023-04-05 02:41:48.173','2023-04-05 02:41:48.173',
207,'CESS','CESS','8678787786786','SBI','87654367','87644389','test',SYSDATETIME());

Insert into CessTransactions
(ReferenceId,VendorId,VendorName,PhoneNumber,AssesmentYear,Amount,ChargeOrPayment,TransactionGeneratedDate,TransactionDate
,ReferenceNumber,ReferenceType,TransactionType,AccountNumber,BankName,UTRNumber,Cheque_No,IFSCCode,CreatedBy,CreatedDate)

values
(203,3,'Raju Constructions','9696969696','2023-2024','160000.0000000000000000','p','2023-04-04 06:46:50.607','2023-04-04 06:46:50.607',
203,'CESS','CESS','0436108024899','Canara Bank DBL','006894657','234567123342331','CNRB0004894','test',SYSDATETIME())


Insert into BankTransactions
 (Transaction_Date,Value_Date,Debit,AccountNo,BankName,RefNo_ChequeNo)
 values
 ('2023-04-05 02:41:48.173','2023-04-05 02:41:48.173','77000.0000000000000000','8678787786786','SBI','87644389')
 select * from cesstransactions order by Id desc
  select * from BankTransactions order by Id desc
