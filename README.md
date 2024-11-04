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

Spin up a postgres database, for example with the provided `docker-compose.yml`. First, copy and rename `.env.example` as `.env` your database details. Then:

```sh
# From root directory
docker compose up
```

Note: 
The `docker-compose.override.yml` exposes the chosen Postgres port for development.
The compose setup also starts "Adminer" as database management tool, reachable at http://localhost:8080.

### With Devcontainer

Copy and rename `.env.example` as `.env` into `.devcontainer` folder and set your database details.

### Initialize Database

#### Via EF Migrations

You can initialize a local database by running the provided EF migrations:

```sh
# From RobotService folder
dotnet ef database update
```

```sh
# From root
dotnet ef database update -p RobotService
```

#### Manually

Create a table `CleaningResult` in your DB:

```sql
CREATE TABLE cleaning_results (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    timestamp timestamp without time zone NOT NULL,
    commands integer NOT NULL,
    result integer NOT NULL,
    duration double precision NOT NULL,
    CONSTRAINT pk_cleaning_results PRIMARY KEY (id)
);
```

Note: You can generate the SQL for the given EF migration via: `dotnet ef migrations script`.


## Release Build

A `Dockerfile` and `docker-compose.prod.yml` are provided to run the API as Release build on port 5000.

First, create a copy of `.env.example` as `.env` and set the values.

Start everything:

```sh
# From root directory
# docker.compose.override.yml is automatically not applied
docker compose -f docker-compose.yml -f docker-compose.prod.yml up
```

You can now reach the API via http://localhost:5000 and provides a SwaggerUI for the API.

If you make changes, make sure to rebuild the container:

```
docker compose -f docker-compose.yml -f docker-compose.prod.yml up --build
```

## Testing

Just run `dotnet test` from root directory. This runs all tests in the solution.

For `RobotService` this means running unit tests and integration tests against an in-memory databse.

## Utilities

The `PathGenerator` project is a small CLI for generating large path samples. Paths are stored under `/paths` in separate `.json` files for each path. The path includes the `uniquePlaces` for each paths, to allow for quickly verifying the `RobotService` results.

In the `scripts` folder, are a PowerShell and Bash script for sending a folder of generated JSON path files to the locally running service.