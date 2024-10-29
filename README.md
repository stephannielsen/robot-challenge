# Robot Challenge

Small service which calculates the unique places cleaned given a set of path a cleaning robot has taken.

The service uses a .NET Minimal API with EntityFramework Core (PostgreSql).

## Prerequisites

* .NET 8
* Docker with Docker Compose

## Local Development

You can run the app with a local .NET installation or with Devcontainer. Once running, the API will be available at http://localhost:5000 and provides a SwaggerUI for the API.

### Without Devcontainer

Copy `RobotService/appsettings.json` as `RobotService/appsettings.Development.json` and set a database connection string:

```json
  "ConnectionStrings": {
    "RobotDb": "Host=<db>;Port=<port>;Username=<username>;Password=<password>;Database=<DbName>"
  }
```

Spin up a postgres database, for example with the provided `docker-compose.postgres.yml`. First, copy and rename `.env.example` as `.env` your database details. Then:

```sh
docker compose up
```

### With Devcontainer

Copy and rename `.env.example` as `.env` into `.devcontainer` folder and set your database details.

## Release Build

A `Dockerfile` and `docker-compose.prod.yml` are provided to run the API as Release build on port 5000.

First, create a copy of `.env.example` as `.env` and set the values.

Start everything:

```sh
# From root directory
docker compose -f docker-compose.yml -f docker-compose.prod.yml up
```

You can now reach the API via http://localhost:5000 and provides a SwaggerUI for the API..