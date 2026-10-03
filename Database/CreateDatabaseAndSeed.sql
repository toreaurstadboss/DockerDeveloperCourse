IF DB_ID('Podcasts') IS NULL
BEGIN
    CREATE DATABASE Podcasts;
END;
GO

"Connecting Podcasts..."

USE Podcasts;
GO

IF OBJECT_ID('dbo.Podcasts', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Podcasts;
END;
GO

PRINT "Dropping table Podcasts..."

CREATE TABLE dbo.Podcasts(
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(100) NOT NULL
);
GO

PRINT "Create table Podcasts...."

INSERT INTO dbo.Podcasts (Title) VALUES
    ('.NET Rocks Podcasts'), 
    ('The .NET Rocks! Podcast covers .NET development topics.'),
    ('Azure Podcasts'),
    ('AWS Podcasts'),
    ('MSDN Daily'),
    ('Python Programmer Podcasts')



PRINT "Inserting to Podcasts table rows.."