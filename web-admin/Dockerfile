# Use a imagem Node para construir o aplicativo
FROM node:16.20.2 as build

# Define o diretório de trabalho dentro do contêiner
WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build --dev

# Using alpine as a soft webserver
FROM nginx:alpine

COPY --from=build /app/dist/web-admin /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]