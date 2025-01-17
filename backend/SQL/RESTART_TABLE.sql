USE [gymmanagement];
GO;

DROP TABLE IF EXISTS [dbo].[Users];
DROP TABLE IF EXISTS [dbo].[Subscriptions];
DROP TABLE IF EXISTS [dbo].[GymVisits];

CREATE TABLE [dbo].[Users] (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) UNIQUE NOT NULL,
    Role VARCHAR(255) DEFAULT 'Customer'
);

CREATE TABLE [dbo].[Subscriptions] (
    SubscribtionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    JoinDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    UserId UNIQUEIDENTIFIER CONSTRAINT FK_SubscriptionsUser FOREIGN KEY REFERENCES Users(UserId) NOT NULL
);

CREATE TABLE [dbo].[GymVisits] (
    GymVisitId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FromDate DATETIME NOT NULL,
    ToDate DATETIME DEFAULT NULL,
    UserId UNIQUEIDENTIFIER CONSTRAINT FK_GymVisitsUser FOREIGN KEY REFERENCES Users(UserId) NOT NULL
);

INSERT INTO [dbo].[Users](UserId, FirstName, LastName, Email, Password, Role) VALUES('97E42B07-29FB-4314-92D1-F9EA190DE1B5', 'Adam', 'Zeggai', 'adam.zeggai@gmail.com', '$2a$12$dupedxw1geh6fConEY2Ug.1KWeDlRdwEMWoVEoQ09ed0TouOaC.fe', 'Admin');