select Id from CommonMaster where CodeType ='ReconcileStatus' and CodeName='Matched' and IsActive=1
Insert into CommonMaster
Values('ReconcileStatus','Pending','Pending', 10, 1);
Insert into CommonMaster
Values('ReconcileStatus','Matched','Matched', 20, 1);
Insert into CommonMaster
Values('ReconcileStatus','Reconciled','Reconciled', 30, 1);
