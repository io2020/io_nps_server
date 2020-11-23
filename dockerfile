# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/Nps.Api/*.csproj ./Nps.Api
RUN dotnet restore -r linux-x64

# copy everything else and build app
COPY src/Nps.Api/. ./Nps.Api/
WORKDIR /source/Nps.Api
RUN dotnet publish -c release -o /app -r linux-x64 --self-contained false --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-amd64
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["./Nps.Api"]
