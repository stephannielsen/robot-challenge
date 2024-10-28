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

You can run a sample Postgres container as database like this or use your own:

```sh
```

### With Devcontainer

Copy and rename `.env.example` as `.env` into `.devcontainer` folder and set tour database details.

Note: If you want to change the Postgres credentials after you have created the devcontainer for the first time, you might need to remove the volumes manually first, otherwise the credentials are not updated.

## Release Build

A `Dockerfile` and `docker-compose.yml` are provided to run the API as Release build on port 5000.

First, create a copy of `.env.example` as `.env` and set the values as described above. The file can be identical to `/.devcontainer/.env`.

Start everything:

```sh
# From root directory
docker compose up
```

You can now reach the API via http://localhost:5000 and provides a SwaggerUI for the API..