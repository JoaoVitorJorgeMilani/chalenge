cd ../../web-admin
docker build -t mottu-webadmin-image -f Dockerfile .
docker run --name mottu-webadmin -p 4201:80 -d mottu-webadmin-image