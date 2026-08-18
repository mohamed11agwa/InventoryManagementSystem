# Fulfillment & Inventory Management Platform

An ASP.NET Core API for managing products, categories, warehouses, inventory, and stock adjustments.

Built with Clean Architecture, CQRS/MediatR, Entity Framework Core, and ASP.NET Core Identity.

-------------------------------------------------

Running the Project

Prerequisites
.NET SDK
SQL Server
Visual Studio or another compatible .NET IDE

Clone
git clone <repository-url>
cd InventoryManagementSystem

Configure the Database

Update the connection string in:
Inventory.Api/appsettings.json

or the appropriate environment-specific configuration.

Apply Migrations
dotnet ef database update

Run
dotnet run

The API can then be accessed through the configured ASP.NET Core URL.

## Test Users

| Role | Email | Password |
|---|---|---|
| Administrator | admin@inventory.local | Password123! |
| Warehouse Operator | operator@inventory.local | Password123! |
| Manager | manager@inventory.local | Password123! |