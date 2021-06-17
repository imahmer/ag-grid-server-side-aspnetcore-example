CREATE TABLE [dbo].[OlympicWinners] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Athlete] NVARCHAR (256) NULL,
    [Age]     TINYINT        NULL,
    [Country] NVARCHAR (256) NULL,
    [Year]    INT            NULL,
    [Date]    NVARCHAR (10)  NULL,
    [Sport]   NVARCHAR (256) NULL,
    [Gold]    TINYINT        NULL,
    [Silver]  TINYINT        NULL,
    [Bronze]  TINYINT        NULL,
    [Total]   TINYINT        NULL,
    CONSTRAINT [PK_OlympicWinners] PRIMARY KEY CLUSTERED ([Id] ASC)
);

