-- Dec 14 2019 - Localization
insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'Locale', 'en-US', 'English', 10, 1, 1),
( 'Locale', 'kn-IN', 'ಕನ್ನಡ', 20, 1, 1),
( 'Locale', 'hi-IN', 'हिन्दी', 20, 1, 1)
go
