# 💍 Professional Jewellery Store API & Frontend

A full-stack, ejewellery store application built with .NET 8 and SvelteKit.

## Key Features

-   **Unified Response Pattern**: Every API call uses a consistent `ServiceResponse<T>` wrapper for predictable results and error handling.
-   **Secure Authentication**: JWT-based login and registration with BCrypt password hashing.
-   **Role-Based Security**: Special admin routes and middleware protect sensitive store management areas.
-   **Environment Driven**: Configurable API endpoints via `.env` files for easy deployment.
-   **Modern UI**: High-performance SvelteKit frontend with smooth transitions and responsive design.

## Tech Stack

### Backend (.NET 8 WebAPI)
-   **Dapper**: Fast and efficient raw SQL mapping.
-   **PostgreSQL**: Robust relational database.
-   **BCrypt.Net**: Secure password hashing.
-   **JWT**: Token-based authentication.

### Frontend (SvelteKit)
-   **Svelte 5**: Modern reactive UI logic.
-   **Tailwind CSS**: Professional styling.
-   **Universal Cookies**: Server-friendly session management.

## 📦 Getting Started

### 1. Database Setup
Run the [database.sql](file:///home/notcool/.gemini/antigravity/brain/97b03cce-5b2e-4730-b2ef-e47bc8b8dcd0/database.sql) script in your PostgreSQL instance to create the tables and seed sample data.

### 2. Backend Config
Update `backend/JewelryStore.Api/appsettings.json` with your PostgreSQL credentials.

```bash
cd backend/JewelryStore.Api
dotnet run
```

### 3. Frontend Config
Create a `frontend/.env` file based on `.env.example`.

```bash
cd frontend
pnpm dev
```

---