
go

drop table if exists dbo.tasks
drop table if exists dbo.projects
drop table if exists dbo.[status]
drop table if exists dbo.[priority]

go

create table dbo.[status]
(
	id int  primary key identity(1,1),
	[description] varchar (30)
)

insert into dbo.[status]([description])
	select 'development' union all
	select 'quality assurance' union all
	select 'user accepted test' union all
	select 'recovery'

go

create table dbo.[priority]
(
	id int  primary key identity(1,1),
	[description] varchar (30)
)

insert into dbo.[priority]([description])
	select 'low' union all
	select 'middle' union all
	select 'high'


go

create table dbo.projects 
(
	id int primary key identity(1,1),
	[name] varchar(64),
	[description] varchar(256),
	created_date datetime default (getdate())
)

create table dbo.tasks 
(
	id int primary key identity(1,1),
	[title] varchar(256),
	[fk_project_id] int foreign key references dbo.projects(id),
	[fk_status_id] int foreign key references dbo.[status](id),
	[fk_priority_id] int foreign key references dbo.[status](id),
	created_date datetime default (getdate())
)