# template-architecture-cqrsflib
Architecture CQRS FullLIB (.NET 8.0)

#### Migrations

Go to "BAYSOFT.Infrastructures.Data" project folder and open cmd

> dotnet ef --startup-project ../BAYSOFT.Presentations.WebAPI migrations add InitialMigrationDefaultDbContext -c DefaultDbContext -o Default/Migrations