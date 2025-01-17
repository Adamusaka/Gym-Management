USE [gymmanagement];
GO;

SELECT * FROM [dbo].[Users]
SELECT * FROM [dbo].[Subscriptions]
SELECT * FROM [dbo].[GymVisits]

SELECT * FROM Subscriptions WHERE UserId = '4223ECA2-DF55-4B23-AAAB-0CA5BF5F1753'

SELECT Users.*, Subscriptions.JoinDate, Subscriptions.EndDate FROM Users INNER JOIN Subscriptions ON Users.UserId = Subscriptions.UserId