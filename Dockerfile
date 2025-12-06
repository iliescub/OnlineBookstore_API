# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Bookstore.API/BookstoreAPI.csproj", "Bookstore.API/"]
COPY ["Bookstore.Application/Bookstore.Application.csproj", "Bookstore.Application/"]
COPY ["Bookstore.Domain/Bookstore.Domain.csproj", "Bookstore.Domain/"]
COPY ["Bookstore.Infrastructure/Bookstore.Infrastructure.csproj", "Bookstore.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Bookstore.API/BookstoreAPI.csproj"

# Copy the rest of the code
COPY . .

# Build the project
WORKDIR "/src/Bookstore.API"
RUN dotnet build "BookstoreAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BookstoreAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Expose the port (Render will set this via PORT env var)
EXPOSE 8080

# Copy the published app
COPY --from=publish /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "BookstoreAPI.dll"]
