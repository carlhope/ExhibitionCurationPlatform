# Exhibition Curation Platform

A Blazor-based application for managing exhibitions and artworks. The project focuses on clean architecture, modular service design, and maintainable code practices.

## 🚀 Setup Instructions (SQL Server)

To run the application locally:

1. **Clone the repository**
    ```bash
    git clone https://github.com/your-repo/exhibition-curation-platform.git
    ```

2. **In `appsettings.json`, configure your SQL Server connection**
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER;Database=ExhibitionDb;Trusted_Connection=True;"
    }
    ```

3. **Apply migrations**
    ```bash
    dotnet ef database update
    ```

4. **Run the application**
    ```bash
    dotnet run
    ```


## Features

- Exhibition creation and editing
- Artwork search functionality
- Form handling and validation
- Modular service layer
- User creation and login
- Identity based page access control

## Tech Stack

- Blazor (.NET)
- Entity Framework Core
- Blazor Identity
- xUnit (Testing)
