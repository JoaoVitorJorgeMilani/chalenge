# Use a imagem base do SDK do .NET para construir o aplicativo
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Define o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copia o arquivo csproj e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante do código-fonte e constrói o aplicativo
COPY . ./
RUN dotnet publish -c Release -o out

# Cria a imagem final com a aplicação publicada
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expor a porta em que o aplicativo estará ouvindo
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

# Comando para iniciar o aplicativo quando o contêiner for iniciado
ENTRYPOINT ["dotnet", "web-admin-back.dll"]