cd ../../web-admin-back
docker build -t mottu-webadminback-image -f Dockerfile .
docker run --name mottu-webadminback -p 5000:5000 --network="host" -d mottu-webadminback-image