# DockerDeveloperCourse

Notes for the Dometrain course [Docker From Zero to Hero: Docker for Developers](https://github.com/dmitri-mamrukov/dometrain-from-zero-to-hero-docker-for-developers/blob/main/README.md).

This repository contains a small Docker-based sample with three parts:

- a Blazor WebAssembly frontend
- an ASP.NET Core Web API
- a SQL Server database seed container

## Repository Overview

The solution is organized around `docker-compose.yaml`:

- `Frontend/` builds the browser app and serves the compiled static site through Nginx.
- `Api/` builds the Web API and publishes it as a containerized .NET app.
- `Database/` contains the SQL Server bootstrap script and a helper shell script that waits for SQL Server before running the seed.

The compose file publishes these ports locally:

- `1234` for the frontend
- `17860` for the API
- `1433` for SQL Server

## How To Run

Start everything with Docker Compose:

```bash
docker compose up --build
```

Useful variants:

```bash
docker compose up -d --build
docker compose up -d --force-recreate api
docker compose up -d --force-recreate frontend
docker compose build --no-cache frontend
docker compose down
```

When the stack is running:

- Frontend: http://localhost:1234
- API: http://localhost:17860/podcasts

## Dockerfiles

### Frontend Dockerfile

`Frontend/Dockerfile` uses the .NET SDK image to publish the Blazor WebAssembly app, then copies the published `wwwroot` output into Nginx.

### API Dockerfile

`Api/DockerCourseWebApi/Dockerfile` restores and publishes the Web API with the .NET SDK image, then runs the app from the .NET ASP.NET runtime image with `dotnet DockerCourseWebApi.dll`.

### Database Dockerfile

`Database/Dockerfile` uses the SQL Server image, copies in the seed script and helper shell script, and runs the shell script on startup.

## Wait-And-Run Script

`Database/wait-and-run.sh` waits for SQL Server to accept connections, then runs the seed SQL with `sqlcmd`.

```bash
#!/bin/bash

set -euo pipefail

SQL_SERVER_HOST="${SQL_SERVER_HOST:-database}"
SQL_SERVER_USER="sa"
SQL_SERVER_PASSWORD="Dometrain#123"
SQL_SCRIPT_PATH="${SQL_SCRIPT_PATH:-/CreateDatabaseAndSeed.sql}"
SQLCMD="/opt/mssql-tools18/bin/sqlcmd"
 
echo "Waiting for SQL Server on ${SQL_SERVER_HOST}..."

ready="false"
for i in {1..500};
do
	if printf "SELECT 1\nGO\n" | "${SQLCMD}" -C -S "${SQL_SERVER_HOST}" -U "${SQL_SERVER_USER}" -P "${SQL_SERVER_PASSWORD}" -d master -b -l 1 >/dev/null 2>&1; then
		ready="true"
		break
	fi

	if (( i % 10 == 0 )); then
		echo "Waiting for SQL Server... attempt ${i}/500"
	fi

	sleep 1
done

if [ "${ready}" != "true" ]; then
	echo "SQL Server was not ready after 500 seconds."
	exit 1
fi

echo "SQL Server is ready. Running ${SQL_SCRIPT_PATH}..."
"${SQLCMD}" -C -S "${SQL_SERVER_HOST}" -U "${SQL_SERVER_USER}" -P "${SQL_SERVER_PASSWORD}" -d master -i "${SQL_SCRIPT_PATH}" -b
```

## Helpful Docker CLI Commands

- `docker compose ps` - show the current service state
- `docker compose logs -f api` - follow API logs
- `docker compose logs -f database-seed` - follow the seed container logs
- `docker compose exec api sh` - open a shell in a running container
- `docker compose build --no-cache frontend` - force a clean frontend rebuild
- `docker compose up -d --force-recreate api` - recreate the API container
- `docker compose down` - stop and remove the stack

## Software To Install

To run this repo locally, install:

- [Visual Studio Code](https://code.visualstudio.com/)
- [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop/)
- [.NET SDK](https://dotnet.microsoft.com/download) if you want to build the projects outside Docker

Tip: Docker Desktop for Windows has a free personal-use version, which is enough for this repo.

## Notes

- The API expects the seeded SQL Server database to be available on the compose network.
- The frontend calls the API over the host port published by Compose, so the browser can reach it directly.
- If you change any Dockerfile or compose setting, rebuild the affected service before testing again.