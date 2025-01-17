docker build -t gym-management-app:latest .

docker tag gym-management-app:latest localhost:5000/gym-management-app:latest


@REM frontend
docker run -d -p 30002:8001 --name gym-management-app --network gym-management-net --restart unless-stopped gym-management-app:latest