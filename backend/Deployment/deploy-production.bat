docker volume create gym-management-vol
docker network create gym-management-net

docker build -t gym-management-api:latest .

@REM baza
docker run -p 30000:1433 --name gym-management-db --network gym-management-net -d -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=a0c0f085-0751-44bb-b370-1e9a1061a379" -v mixeditor-vol:/var/opt/mssql --restart unless-stopped mcr.microsoft.com/mssql/server:2022-latest

@REM backend
docker run -d -p 30001:8000 --name gym-management-api --network gym-management-net --restart unless-stopped gym-management-api:latest