Here is the translation in English:

## MyMoney API - Manage your personal finances with ease!

This is a RESTful API built with ASP.NET Core and Entity Framework Core that provides a set of endpoints to manage your income, expenses, and get a monthly summary of your finances.

### Main Features

-   **Authentication:** Supports user authentication using ASP.NET Core Identity with custom entities (`PersonWithAccess` and `AccessProfile`).
-   **Income Management:**
    -   Create, list, search, update, and delete income.
    -   List income by a specific month/year.
-   **Expense Management:**
    -   Create, list, search, update, and delete expenses.
    -   Categorize expenses (with a predefined default category).
    -   List expenses by a specific month/year.
-   **Monthly Summary:**
    -   Get a complete financial summary for a specific month, including:
        -   Total income.
        -   Total expenses.
        -   Final balance.
        -   Spending by category.
-   **Swagger Documentation:** The API has complete interactive documentation using Swagger, available at `/swagger` on the project's root.

### Technologies Used

-   **Backend:**
    -   C#
    -   ASP.NET Core 8
    -   Entity Framework Core
    -   Microsoft.AspNetCore.Identity
    -   Swashbuckle (for Swagger documentation)
-   **Database:**
    -   SQL Server (with configuration via `appsettings.json` or a direct connection string).
-   **Tools:**
    -   Visual Studio (or your preferred IDE).
    -   Git (for code versioning).

### Installation and Execution

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/Matchiga/MyMoney.git
    ```

2.  **Restore NuGet packages:**
    -   Open the project in Visual Studio or use the `dotnet restore` command in the project folder.

3.  **Configure the database:**
    -   Update the connection string in the `appsettings.json` file or within the `MDContext` class to point to your SQL Server instance.
    -   Run the Entity Framework migrations to create the database:
        ```bash
        dotnet ef database update
        ```

4.  **Run the application:**
    -   Use the `dotnet run` command or run the project from Visual Studio.
    -   Access the Swagger documentation at: `https://localhost:7168/swagger` (check the correct port in `launchSettings.json`).

### Important Code Points

-   **`DAO<T>`:** A generic Data Access Object (DAO) class that provides basic CRUD methods to interact with the database.
-   **Endpoints:** Implemented using ASP.NET Core's Minimal APIs for better readability and conciseness.
-   **Authentication:** The `/auth` endpoint (configured with `MapIdentityApi`) provides endpoints for user registration, login, and management.
-   **Error Handling:** Custom middleware to return more user-friendly error messages (status codes 401 and 403).

### Next Steps

-   Implement unit and integration tests.
-   Add more robust data validation to the endpoints.
-   Create a user interface (frontend) to interact with the API.

## Contributions

Contributions are welcome! Feel free to open issues to report bugs, request new features, or submit pull requests.
