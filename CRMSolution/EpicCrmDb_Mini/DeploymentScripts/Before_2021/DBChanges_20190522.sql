-- 22.5.2019

ALTER TABLE dbo.Entity
ADD IsActive BIT NOT NULL DEFAULT 1

-- SQL To disable duplicate Entities
DECLARE @ExtraEntity TABLE
( Id BIGINT,
  EntityName NVARCHAR(50),
  RN INT
)

;with entityCTE(entityType, entityName, EmployeeId, landSize, contactCount, entityDate, cc)
AS
(
select EntityType, EntityName, EmployeeId, landSize, contactCount, entityDate, count(*)
from dbo.Entity
group by EntityType, EntityName, EmployeeId, landSize, contactCount, entityDate
having count(*) > 1
)
INSERT INTO @ExtraEntity (Id, EntityName, RN)
SELECT e.Id, e.EntityName,
--SELECT e.id, e.EmployeeId, e.EntityType, e.EntityName, e.EntityDate,
ROW_NUMBER() OVER (Partition by e.entityName, e.entityDate order by e.entityName, e.EntityDate, e.Id) as RN
FROM dbo.Entity e
INNER JOIN entityCTE cte on e.EntityType = cte.entityType
AND e.EntityName = cte.entityName
AND e.EmployeeId = cte.EmployeeId
and e.EntityDate = cte.entityDate
--order by RN
--order by e.EntityName, e.EntityDate, e.Id

--select * from @ExtraEntity

DECLARE @ReplaceTable TABLE
( EntityId BIGINT,
  EntityName NVARCHAR(50),
  ActivityId BIGINT,
  ReplacementEntityId BIGINT
)

; with RepCTE(EntityId, EntityName, ActivityId)
AS
(
SELECT e.Id, e.EntityName, a.Id -- a.clientCode, e.Id 'EntityId', a.* 
FROM dbo.Activity a
inner join @ExtraEntity e on a.ClientName = e.EntityName
WHERE a.clientCode <> ''
and a.activityType <> 'Order' and a.activityType <> 'Payment' and a.ActivityType <> 'OrderReturn'
and convert(nvarchar(10), e.id) = a.ClientCode
)
--select * from RepCTE
INSERT INTO @ReplaceTable
(EntityId, EntityName, ActivityId, ReplacementEntityId)
SELECT cte.*, ee.Id 'ReplacementEntityId'
from RepCTE cte
INNER JOIN @ExtraEntity ee on cte.EntityName = ee.EntityName COLLATE Latin1_General_CS_AS
AND ee.Id < cte.EntityId
AND ee.RN = 1

--SELECT * from @ReplaceTable

-- Activity Replace Table
--SELECT rt.ActivityId, a.ClientCode, rt.ReplacementEntityId FROM 
--@ReplaceTable rt
--INNER JOIN dbo.Activity a on rt.ActivityId = a.Id

UPDATE dbo.Activity
SET ClientCode = convert(nvarchar(10), rt.ReplacementEntityId)
FROM dbo.Activity a
INNER JOIN @ReplaceTable rt on a.Id = rt.ActivityId

-- These Entity have to be deleted
--SELECT * FROM @ExtraEntity WHERE RN > 1
UPDATE dbo.Entity
SET IsActive = 0
FROM dbo.Entity e
INNER JOIN @ExtraEntity ee on e.Id = ee.Id
AND ee.RN > 1

---=======================================