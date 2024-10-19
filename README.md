# KosHome

## Overview

### Sequence Diagrams

* Hello World
    * [Hello World](./diagrams/helloworld.png)

#### PlantUML Generator

```
rm -f diagrams/*.png
docker run --rm -v $(pwd):/data hrektts/plantuml plantuml diagrams/*
```

## Build and Run

You can build and run the application using one of the following methods.

### Docker (same as build server) - preferred

```
cd docker
docker-compose up --build
```

### Dotnet CLI
```
dotnet run --project src/KosHome.WebApi/KosHome.WebApi.csproj
```

### Dotnet CLI

```
dotnet test --filter "Category=Unit|Category=Integration|Category=Component" KosHome.sln 
```