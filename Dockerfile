# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy backend source
COPY Backend/ ./Backend/

# Restore and build backend
WORKDIR /src/Backend/ProjectTemplate2024.Api
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Use official Node.js image for frontend build
# FROM node:18 AS frontend-build
# WORKDIR /src
# COPY Client/ ./Client/
# WORKDIR /src/Client
# RUN npm ci
# RUN npm run build-only

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Copy backend publish output
COPY --from=build /app/publish .

# Copy frontend build output to backend wwwroot (assuming SPA static files)
# COPY --from=frontend-build /src/Client/dist ./wwwroot

# Expose backend port
EXPOSE 5000

# Set environment variables (override in docker-compose)
ENV ASPNETCORE_URLS=http://+:5000

# Start backend
ENTRYPOINT ["dotnet", "ProjectTemplate2024.Api.dll"]
