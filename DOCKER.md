# ProjectTemplate2024 Docker Setup

This project uses Docker to run the backend (ASP.NET Core), frontend (Vue 3), and SQL Server for development and testing.

## Prerequisites

- Docker Desktop (Windows/Mac) or Docker Engine (Linux)
- .NET 10 SDK (for local development)
- Node.js 18+ (for local frontend development)

## Usage

### 1. Build and Run All Services

```
docker-compose up --build
```

- Backend: http://localhost:5000
- SQL Server: localhost:1433 (user: sa, password: see `.env.docker.example`)

### 2. Environment Variables

- Copy `.env.docker.example` to `.env` and adjust as needed.
- The backend uses the connection string from the environment.

Example (PowerShell):

```
Copy-Item .env.docker.example .env
docker-compose up --build
```

Example (bash):

```
cp .env.docker.example .env
docker-compose up --build
```

JWT secret for containers

The API requires a non-empty `JwtSettings:Secret` (used to sign tokens). When running in Docker the local `dotnet user-secrets` store is not available, so you must set the secret in your runtime `.env`.

Generate a secure secret and add it to `.env` (replace the placeholder in the example):

PowerShell (Windows):

```powershell
# generate a 32-byte base64 secret
[Convert]::ToBase64String((1..32|%{Get-Random -Maximum 256}) -as [byte[]]) > jwt.txt
Get-Content jwt.txt
```

Linux/macOS with openssl:

```bash
openssl rand -base64 32
```

Then open `.env` and set:

```
JwtSettings__Secret=YOUR_GENERATED_SECRET
```

Restart containers after editing `.env`:

```bash
docker-compose down
docker-compose up --build
```

### 3. Frontend

- The frontend is built and served as static files by the backend (SPA mode).
- For local development, run the frontend separately:
  ```
  cd Client
  npm ci
  npm run dev
  ```

### 4. Database

- SQL Server data is persisted in a Docker volume (`sqlserver_data`).
- Connect using any SQL client at `localhost:1433`.

### 5. Stopping and Cleaning Up

```
docker-compose down
```

- To remove volumes (delete all data):
  ```
  docker-compose down -v
  ```

## Notes

- For production, update secrets and review Dockerfile for optimizations.
