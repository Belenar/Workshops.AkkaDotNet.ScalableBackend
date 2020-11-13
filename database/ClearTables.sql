USE AkkaNetResults;

DELETE FROM dbo.MeterReadings;

USE AkkaPersistence;

DELETE FROM dbo.EventJournal;
DELETE FROM dbo.SnapshotStore;