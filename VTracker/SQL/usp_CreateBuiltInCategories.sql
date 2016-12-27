alter proc dbo.usp_CreateBuiltInCategories
(
	@AccountId varchar(128)
)

/*

usp_CreateBuiltInCategories 'e76ba5a5-95fd-4623-b49b-18d6ff36de1e'

*/

as

insert into Category (Description, AccountId, BuiltIn, DateCreated, Deleted)
select b.Description, @AccountId, 1, getdate(), 0
from BuiltInCategories b
