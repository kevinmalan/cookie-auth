﻿CREATE TABLE [dbo].[RefreshToken]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[LookupId] UNIQUEIDENTIFIER NOT NULL,
	[AccessTokenId] NVARCHAR(100) NOT NULL,
	[ProfileId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Profile] ([Id]),
	[Value] UNIQUEIDENTIFIER NOT NULL,
	[ExpiresOn] DATETIMEOFFSET NOT NULL
);
GO
CREATE INDEX [IX_LookupId]
ON [dbo].[RefreshToken] ([LookupId])
GO
