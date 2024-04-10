docker build -t mottu-rabbitmq-image -f Dockerfile .
docker run -d --name mottu-rabbitmq -p 5672:5672 -p 15672:15672 mottu-rabbitmq-image