﻿CREATE TABLE [dbo].[RefreshToken]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[LookupId] UNIQUEIDENTIFIER UNIQUE NOT NULL,
	[AccessTokenId] UNIQUEIDENTIFIER NOT NULL,
	[ProfileId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Profile] ([Id]),
	[Value] NVARCHAR(64) NOT NULL,
	[ExpiresOn] DATETIMEOFFSET NOT NULL,
	[CreatedOn] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
);
GO
CREATE INDEX [IX_LookupId]
ON [dbo].[RefreshToken] ([LookupId])
GO
CREATE INDEX [IX_ProfileId]
ON [dbo].[RefreshToken] ([ProfileId])
GO
CREATE INDEX [IX_Value]
ON [dbo].[RefreshToken] ([Value])
GO
