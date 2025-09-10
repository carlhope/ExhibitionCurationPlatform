# Exhibition Curation Platform

A Blazor-based application for managing exhibitions and artworks. The project focuses on clean architecture, modular service design, and maintainable code practices.

## What You Can Do with This App
This platform lets you explore artworks from major museums and build your own personal exhibitions using pieces you've created. As a user, you can:
- Browse artworks from sources like the Met Museum and Harvard Art Museums
- Create your own artworks and save them into personal exhibitions
- Organise multiple exhibitions, each with its own theme, layout, and description
- Add or remove your own artworks from exhibitions at any time
- View your exhibitions and see all the pieces you've curated
- Log in securely to manage your personal collection
It’s designed to feel like your own virtual gallery—where you’re the artist and the curator.


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

## Future Enhancements

- **Support for Artwork Reuse Across Exhibitions**  
  Currently, each artwork belongs to a single exhibition. A future update could introduce a many-to-many relationship, allowing artworks to appear in multiple curated collections.

- **Dynamic Layout Rendering**  
  The `Layout` field is currently descriptive. Future versions could use this value to control how artworks are visually arranged (e.g., grid, carousel, timeline).

- **Theme-Based Filtering and Discovery**  
  The `Theme` field could be used to group exhibitions and enable users to browse by artistic style or concept (e.g., Modern, Minimalist, Classic).

- **Enhanced Artwork Creation Tools**  
  Expand the user artwork creation flow to include image uploads, tagging, or richer metadata to support more expressive exhibitions.
