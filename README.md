A complete library management system built with ASP.NET Core MVC and SQL Server.

## Prerequisites

- Visual Studio 2022
- SQL Server 2019 or later
- .NET 8.0 SDK

## Database Setup

1. Open SQL Server Management Studio
2. Right-click on Databases
3. Select "Restore Database"
4. Choose Device and navigate to Database/LibrarySystem.bak
5. Click OK to restore the database

## Configuration

1. Update the connection string in `appsettings.json`:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=LibrarySystem;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

## Installation
Clone the repository
BASH

git clone https://github.com/yourusername/LibrarySystem.git
Navigate to the project directory
BASH

cd LibrarySystem
Restore NuGet packages
BASH

dotnet restore
Run the application
BASH

dotnet run
Features
Book Management (CRUD operations)
Member Management
Borrowing System
Fine Calculation
Search Functionality
Dashboard with Statistics
Database Objects
Stored Procedures
sp_SearchBooks
sp_BorrowBook
sp_ReturnBook
sp_GetOverdueBooks
sp_GetMemberActiveBorrowings
Functions
fn_CalculateFine
Views
VW_BorrowingDetails
