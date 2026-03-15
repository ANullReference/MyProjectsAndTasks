CREATE TABLE [dbo].[projects] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [name]         VARCHAR (64)  NULL,
    [description]  VARCHAR (256) NULL,
    [created_date] DATETIME      DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

