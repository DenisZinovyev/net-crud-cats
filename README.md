# Crud Cats NestJS

## Description

Crud Cats .Net Core MS is an example of Crud Cats compatible backend based on [.Net Core](https://docs.microsoft.com/dotnet/core/about) framework
that can be used as a boilerplate for .Net Core based applications.

Note that MS here means Microservices. It uses code decomposition based on the microservices architecture approach.

## What is included

* Scripts for building, testing, starting app in different modes.
* Safe-as possible static typing.
* IOC-based integration framework.
* Modularization and naming approach.
* Logger module based on Serylog.
* Persistence module based on Entity Framework Core and Repository/UoW pattern.
* Declarative and type-safe validation and serialization.
* Declarative and type-safe controllers/routes configuration.
* Declarative and type-safe domain/persistent model configuration. Type-safe as possible repositories and query-builders.
* Ready to use User management and authentication modules. Declarative ACL configuration.
* Sample Cats/Breeds modules that illustrate how to write domain-specific code.

## Persistence

For persistence this backend uses Entity Framework Core with Posgress backend and requires access to pre-configured Posgress instance.
To start postgress compatible with default settings in docker use the following command:

```bash
docker run --name crudcats-postgres -e POSTGRES_DB=crudcats -e POSTGRES_USER=crudcats -e POSTGRES_PASSWORD=crudcats1 -p 5432:5432 -d postgres
```

## Build the app

```bash
npm run build
```

## Run the app

```bash
# Run in development mode
$ npm run start

# Run in development mode, whatch for changes and automatically restart
$ npm run start:dev

# Run in production mode
$ npm run start:prod
```

## Test the app

```bash
# Run unit tests
$ npm run test

# Run e2e tests
$ npm run test:e2e

# Run tests and produce coverage report
$ npm run test:cov
```

## Development tools

```bash
# Run prettier to auto-format all soruce code
$ npm run format

# Produce linter report
$ npm run lint
```