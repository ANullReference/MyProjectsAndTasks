CREATE TABLE [dbo].[tasks] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [title]          VARCHAR (256) NULL,
    [fk_project_id]  INT           NULL,
    [fk_status_id]   INT           NULL,
    [fk_priority_id] INT           NULL,
    [created_date]   DATETIME      DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([fk_priority_id]) REFERENCES [dbo].[status] ([id]),
    FOREIGN KEY ([fk_project_id]) REFERENCES [dbo].[projects] ([id]),
    FOREIGN KEY ([fk_status_id]) REFERENCES [dbo].[status] ([id])
);

