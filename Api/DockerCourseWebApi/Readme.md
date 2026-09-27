#Docker course web api - Dometrain 


## spin up a sql server container :


```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Dometrain#123" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
```


Sql to run 

```sql

-- @mssql Chat Query Editor (localhost::sa)
-- Created by GitHub Copilot in VSCode MSSQL - review carefully before executing
CREATE DATABASE [Podcasts];

USE Podcasts 


CREATE TABLE Podcasts(
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(MAX) NOT NULL
)

INSERT INTO Podcasts (Title) VALUES 
('Unhandled Exception Podcast'),
('Developer Weekly Podcast'),
('.NET Rocks Weekly Podcast'),
('Azure  Weekly Podcast'),
('AWS Daily Podcast'),
('Nerds And World Domination Weekly Podcast'),
('Math and IT Weekly Podcast'),
('Frontend happy hour Weekly Podcast'),
('Backend Happy hour Weekly Podcast')


```

## Docker - Build the project from Dockerfile

docker build -f .\DockerCourseWebApi\Dockerfile -t dockercoursewebapi:latest .