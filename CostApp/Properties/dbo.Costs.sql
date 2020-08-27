CREATE TABLE [dbo].[Costs] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [DateTime] DATETIME      NULL,
    [Category] NVARCHAR (50) NULL,
    [Cost]     INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

