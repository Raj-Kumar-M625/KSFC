CREATE TABLE HolidayList (
Id bigint Not Null Identity,
AreaCode nvarchar(10) Not Null,
[Date]  datetime2(7) Not Null,
[Description]  nvarchar(50) Not Null,
StartDate  datetime2(7) Not Null,
EndDate  datetime2(7) Not Null,
);