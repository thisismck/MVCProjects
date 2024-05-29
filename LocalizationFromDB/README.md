# ASP.NET MVC Localization from Database

This project demonstrates how to implement localization in an ASP.NET MVC application using a database to store localization resources.

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Database Setup](#database-setup)
- [Installation](#installation)
- [Usage](#usage)
- [License](#license)

## Features

- Store localization resources in a database.
- Dynamically fetch localized strings based on the current culture.
- Easily manage and update localization resources.

## Prerequisites

- .NET 8 or later
- SQL Server or any other database supported by Entity Framework
- Visual Studio 2022 or later (optional, but recommended)

## Database Setup

1. Create a database for your localization resources.
2. Use the following SQL script to create the `LocalizationResources` table:

    ```sql
    CREATE TABLE LocalizationResources (
        Id INT PRIMARY KEY IDENTITY,
        ResourceKey NVARCHAR(256) NOT NULL,
        Value NVARCHAR(MAX) NOT NULL,
        Culture NVARCHAR(10) NOT NULL
    );
    ```

3. Insert your localization data into the `LocalizationResources` table.

## Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/thisismck/MVCProjects.git
    cd MVCProjects
    ```

2. Open the solution in Visual Studio.

3. Update the connection string in `appsettings.json` to point to your database:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
    }
    ```

4. Restore the NuGet packages:

    ```bash
    dotnet restore
    ```

5. Apply any pending migrations to your database:

    ```bash
    dotnet ef database update
    ```

## Usage

1. Run the application:

    ```bash
    dotnet run
    ```

2. Open your browser and navigate to `https://localhost:5001`.

3. The `HomeController` demonstrates how to use the `IStringLocalizer` to fetch localized strings.

    ```csharp
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = _localizer["WelcomeMessage"];
            return View();
        }
    }
    ```

4. In your views, you can use the localizer to display localized strings:

    ```html
    @inject IStringLocalizer<HomeController> Localizer

    <h1>@Localizer["WelcomeMessage"]</h1>
    ```

## License

This project is licensed under the GPL V3 License. See the [LICENSE](LICENSE) file for details.