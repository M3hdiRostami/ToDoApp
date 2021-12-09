
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
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

