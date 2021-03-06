#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN  apt-get update \
  && apt-get install -y wget \
  && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ToDoAPI.csproj", ""]
RUN dotnet restore "./ToDoAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ToDoAPI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "ToDoAPI.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ToDoAPI.dll", "--environment=Development"]