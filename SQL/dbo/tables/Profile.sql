﻿CREATE TABLE [dbo].[Profile]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[LookupId] UNIQUEIDENTIFIER UNIQUE NOT NULL,
	[Username] NVARCHAR(100) UNIQUE NOT NULL,
	[PasswordHash] NVARCHAR(64) NOT NULL,
	[Salt] NVARCHAR(64) NOT NULL,
	[Role] NVARCHAR(50) NOT NULL,
	[CreatedOn] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
);
GO
CREATE INDEX [IX_LookupId]
ON [dbo].[Profile] ([LookupId])
GO
CREATE INDEX [IX_Username]
ON [dbo].[Profile] ([Username])
GO