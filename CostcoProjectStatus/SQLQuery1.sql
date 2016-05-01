create table ProjectUpdate
(
    ProjectUpdateID uniqueidentifier,
    ProjectID uniqueidentifier,
    Subject varchar(400),
    Body varchar(max),
    primary key (ProjectUpdateID),
    foreign key (projectID) references Project (ProjectID)
);


create table StatusUpdate
(
    ProjectID uniqueidentifier,
    ProjectUpdateID uniqueidentifier,
    PhaseID int,
    VerticalID int,
    RecordDate smalldatetime,
    UpdateKey nvarchar(100),
    UpdateValue nvarchar(max)
    primary key (ProjectID, ProjectUpdateID, UpdateKey),
    foreign key (ProjectID) references Project (ProjectID),
    foreign key (PhaseID) references Phase (PhaseID),
    foreign key (VerticalID) references Vertical (VerticalID),
    foreign key (ProjectUpdateID) references ProjectUpdate (ProjectUpdateID)
);

