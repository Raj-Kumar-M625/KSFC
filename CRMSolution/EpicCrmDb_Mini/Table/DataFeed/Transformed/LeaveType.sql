CREATE TABLE LeaveType (
Id bigint Not Null Identity,
EmployeeCode nvarchar(10) Not Null,
LeaveType nvarchar(20) Not Null,
TotalLeaves int Not Null,
StartDate  datetime2(7) Not Null,
EndDate  datetime2(7) Not Null,
);