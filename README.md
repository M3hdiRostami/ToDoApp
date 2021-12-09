# ToDoApp

# Simple ToDoApp Api with .NET Core and CouchBase

## Prerequisites
To run prebuilt project, you will need:

- Couchbase 7 Installed
- [.NET SDK v5+](https://dotnet.microsoft.com/download/dotnet/5.0) installed 


### Install Dependencies 

```sh
cd ./ToDoApp
dotnet restore
```

### Database Server Configuration

All configuration for communication with the database is stored in the appsettings.Development.json and  appsettings.testing.json file.  This includes the connection string, username, password, bucket name, colleciton name, and scope name.
you will need to change username:password to devuser:658965895 before running the application.
> **Note: **make sure that "devuser" user role is Full Admin for executing db init Service succesfully

## Running The Application

*Couchbase 7 must be installed and running on localhost (http://127.0.0.1:8091) prior to running hte the ASP.NET app.  

At this point the application is ready and you can run it:

```sh
dotnet run 
```
You can launch your browser and go to the [Swagger start page](https://localhost:5001/swagger/index.html).

## Running The Tests

To run the standard integration tests, use the following commands:

```sh
cd ./ToDoApp/Tests
dotnet restore
dotnet test
```


