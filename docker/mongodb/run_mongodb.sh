docker build -t mottu-mongodb-image -f Dockerfile .
docker run --name mottu-mongodb -p 27017:27017 -d mottu-mongodb-image
