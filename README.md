# Robot Challenge

Small service which calculates the unique places cleaned given a set of path a cleaning robot has taken.

The service uses a .NET Minimal API with EntityFramework Core (PostgreSql).

## Prerequisites

* .NET 8
* Docker with Docker Compose

## Get Started - Development

Initialize the database using EF Core:

```
dotnet ef database update
```

This runs the initial database migrations to setup the database as needed.

Build the app:

```
dotnet build
```