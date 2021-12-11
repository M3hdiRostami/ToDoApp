# ToDoApp

# Simple ToDoApp Api with .NET Core and CouchBase

## Prerequisites
To run prebuilt project, you will need:

- Couchbase 7 Installed 
- [.NET SDK v5+](https://dotnet.microsoft.com/download/dotnet/5.0) installed 

**OR:**

- Docker Installed
- WSL linux sub system Installed (for other os users)





## Install Dependencies

#### on local machine directly

```sh
cd ./ToDoApp
dotnet restore ToDo.sln
dotnet build ToDo.sln

```

#### via Docker on containers

```sh
cd ./ToDoApp
docker-compose build
```

### Database Server Configuration

All configuration for communication with the database is stored in the appsettings.Development.json and  appsettings.testing.json file.  This includes the connection string, username, password, bucket name, colleciton name, and scope name.
if you will run app on local machine directly (with out containers) you should update "Couchbase" section parameters in appsettings files as below :
```sh
{
 "ConnectionString": "couchbase://127.0.0.1",
 "RestEndpoint": "http://127.0.0.1:8091/"
}
 ```
> Note: make sure that "devuser" user role is Full Admin for executing db init Service succesfully




## Running The Application


#### on local machine

```sh
cd ./ToDoApp
dotnet run ToDoAPI.csproj
```

#### on Docker containers

```sh
cd ./ToDoApp
docker-compose up
```

-  In first step hit to http://127.0.0.1:8091 to do initial settings and creating username:password:role as devuser:658965895:FullAdmin 
-  In second step we will restart our todoapp_web container,so Web app will gain access to couchbase server for running initial service.

```sh
cd ./ToDoApp
docker restart todoapp_web
```
At this point the application is ready ,   You can launch your browser and go to the
- [Swagger  page on local machine](https://localhost:5001/swagger/index.html).
- [Swagger page on Docker container](http://localhost/swagger/index.html).

------------


## Running The Tests

To run the standard integration and unit tests, use the following commands:

```sh
cd ./ToDoApp/Tests
dotnet restore
dotnet test
```


