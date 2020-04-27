﻿CREATE TABLE [dbo].[Tasks]
(
	[ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[SearchEngine] NVARCHAR(20) NOT NULL,
	[LocationCode] INT NOT NULL,
	[LocationName] NVARCHAR(150) NOT NULL,
	[Domain] NVARCHAR(500) NOT NULL,
	[Keywords] NVARCHAR(MAX) NOT NULL,
	[Position] INT NOT NULL
)