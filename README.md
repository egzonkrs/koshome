# KosHome

## Build and Run

You can build and run the application using one of the following methods.

#### Adding a new migration
```
dotnet ef migrations add NewMigrationName --project src/KosHome.Infrastructure --startup-project src/KosHome.Api
```

#### Applying existing migrations
```
dotnet ef database update --project src/KosHome.Infrastructure --startup-project src/KosHome.Api
```