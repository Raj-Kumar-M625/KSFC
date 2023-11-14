-- Script to archive Tracking table data
-- Sep 01 2018 Ajay Aggarwal

CREATE TABLE [dbo].[Tracking_Archive](
	[Id] [bigint] NOT NULL,
	[ChainedTrackingId] [bigint] NULL,
	[EmployeeDayId] [bigint] NOT NULL,
	[ActivityId] [bigint] NOT NULL,
	[At] [datetime2](7) NOT NULL,
	[BeginGPSLatitude] [decimal](19, 9) NOT NULL,
	[BeginGPSLongitude] [decimal](19, 9) NOT NULL,
	[EndGPSLatitude] [decimal](19, 9) NOT NULL,
	[EndGPSLongitude] [decimal](19, 9) NOT NULL,
	[BeginLocationName] [nvarchar](128) NULL,
	[EndLocationName] [nvarchar](128) NULL,
	[BingMapsDistanceInMeters] [decimal](19, 5) NOT NULL DEFAULT ((0)),
	[GoogleMapsDistanceInMeters] [decimal](19, 5) NOT NULL DEFAULT ((0)),
	[LinearDistanceInMeters] [decimal](19, 5) NOT NULL DEFAULT ((0)),
	[DistanceCalculated] [bit] NOT NULL DEFAULT ((0)),
	[IsMilestone] [bit] NOT NULL DEFAULT ((0)),
	[IsStartOfDay] [bit] NOT NULL DEFAULT ((0)),
	[IsEndOfDay] [bit] NOT NULL DEFAULT ((0)),
	[LockTimestamp] [datetime2](7) NULL,
	[ActivityType] [nvarchar](50) NOT NULL DEFAULT ('')
	)
GO


DECLARE @cutOffDate DATE = '2018-07-01'
insert into dbo.Tracking_Archive
SELECT  [Id]
      ,[ChainedTrackingId]
      ,[EmployeeDayId]
      ,[ActivityId]
      ,[At]
      ,[BeginGPSLatitude]
      ,[BeginGPSLongitude]
      ,[EndGPSLatitude]
      ,[EndGPSLongitude]
      ,[BeginLocationName]
      ,[EndLocationName]
      ,[BingMapsDistanceInMeters]
      ,[GoogleMapsDistanceInMeters]
      ,[LinearDistanceInMeters]
      ,[DistanceCalculated]
      ,[IsMilestone]
      ,[IsStartOfDay]
      ,[IsEndOfDay]
      ,[LockTimestamp]
      ,[ActivityType]
  FROM [dbo].[Tracking]
  WHERE [AT] < @cutOffDate
-- 2286961 rows copied

  -- delete distance cal error log data

  delete dc
  from dbo.DistanceCalcErrorLog dc
  inner join dbo.tracking t on t.id = dc.trackingId
  and t.[at] < @cutOffDate
  

  UPDATE dbo.tracking set ChainedTrackingId = null 
  where [at] < @cutOffDate
go

DECLARE @cutOffDate DATE = '2018-07-01'
   DELETE FROM dbo.tracking where [at] < @cutOffDate
   and id not in 
   (
   select t1.ChainedTrackingId from dbo.tracking t1
   inner join dbo.tracking t2 on t1.ChainedTrackingId = t2.Id
   and t2.[at] < @cutOffDate
  )

-- running above query in steps
DECLARE @cutOffDate DATE = '2018-07-01'
   DELETE FROM dbo.tracking where [at] < @cutOffDate
   and id not in 
(
2308347,2303939,2308354,2308354,
2292982,2308391,2308528,2292941,
2301654,2308669,2308759,2292080,
2293544,2308838,2308872,2293341,
2308898,2294624,2309023,2309067,
2309104,2293281,2309251,2309331,
2309490,2309535,2297153,2292075,
2310390,2291801,836987,2292402,
2311664,
2292978,
2292337,
2312230,
2292233,
2293020,
2292464,
2312548,
2293146,
2312674,
2293402,
2293930,
2303957,
2312946,
2293291,
2313194,
2293456,
2313349,
1553395,
2313716,
2292528,
2293295,
2316963,
2291552,
18708,
2275780,
573963,
539589,
523975,
328036,
2294609,
2348242,
2397047,
778850,
2444029,
2258919,
736986,
396082,
806953,
865880,
823504,
2210228,
637489,
2725985,
811467,
528614,
808269,
2870540,
319029,
248517,
45686,
3553,
250564,
2933771,
201041,
484922,
3017732,
591309,
573716,
560461,
61123,
2995178,
483218,
832372,
821707,
556332,
613193,
491678,
800808,
471668
)
