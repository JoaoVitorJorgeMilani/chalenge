docker build -t mottu-redis-image -f Dockerfile . 
docker run -d --name mottu-redis -p 6379:6379 mottu-redis-image