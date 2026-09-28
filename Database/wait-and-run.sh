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
