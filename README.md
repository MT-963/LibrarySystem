Library Management System

A complete library management system built with ASP.NET Core MVC and SQL Server.

Prerequisites

Visual Studio 2022

SQL Server 2019 or later

.NET 8.0 SDK

Database Setup

Open SQL Server Management Studio.

Right-click on Databases.

Select Restore Database.

Choose Device, then navigate to Database/LibrarySystem.bak.

Click OK to restore the database.

Configuration

Open the appsettings.json file.

Update the connection string as shown below:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=LibrarySystem;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

Installation

Clone the Repository:

git clone https://github.com/yourusername/LibrarySystem.git

Navigate to the Project Directory:

cd LibrarySystem

Restore NuGet Packages:

dotnet restore

Run the Application:

dotnet run

Features

Book Management:

Create, Read, Update, Delete (CRUD) operations for books.

Member Management:

Manage library members and their details.

Borrowing System:

Issue and return books.

Fine Calculation:

Automatically calculate overdue fines.

Search Functionality:

Search for books and members.

Dashboard with Statistics:

Overview of library data and analytics.

Database Objects

Stored Procedures

sp_SearchBooks: Search for books based on title, author, or genre.

sp_BorrowBook: Handle the borrowing process.

sp_ReturnBook: Handle the returning process.

sp_GetOverdueBooks: Retrieve overdue books.

sp_GetMemberActiveBorrowings: Get active borrowings for a specific member.

Functions

fn_CalculateFine: Calculate overdue fines based on the due date and return date.

Views

VW_BorrowingDetails: View detailed borrowing records.

Contributing

Contributions are welcome! Please follow these steps:

Fork the repository.

Create a new branch for your feature/bug fix.

Commit your changes and push to your fork.

Submit a pull request to the main repository.

License

This project is licensed under the MIT License.

