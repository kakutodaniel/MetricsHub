# -------------------------
# Build stage
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution / project files first for better layer caching
COPY *.slnx ./

# Copy project files (NO src folder)
COPY MetricsHub.Presentation/*.csproj MetricsHub.Presentation/
COPY MetricsHub.Application/*.csproj MetricsHub.Application/
COPY MetricsHub.Infrastructure/*.csproj MetricsHub.Infrastructure/

# Restore dependencies
RUN dotnet restore MetricsHub.Presentation/MetricsHub.Presentation.csproj

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish MetricsHub.Presentation/MetricsHub.Presentation.csproj \
    -c Release -o /publish /p:UseAppHost=false

# -------------------------
# Runtime stage
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /publish .

# If your app listens on a different port, adjust this
EXPOSE 8080

# Set environment (optional but common)
ENV ASPNETCORE_URLS=http://+:8080

# Start the app (replace with your DLL name)
ENTRYPOINT ["dotnet", "MetricsHub.Presentation.dll"]