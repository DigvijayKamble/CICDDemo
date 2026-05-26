# =========================
# BUILD STAGE
# =========================

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project file first
COPY ["MySampleApi.csproj", "./"]

# Restore dependencies
RUN dotnet restore "MySampleApi.csproj"

# Copy remaining source code
COPY . .

# Build application
RUN dotnet build "MySampleApi.csproj" -c Release -o /app/build

# Publish application
RUN dotnet publish "MySampleApi.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# RUNTIME STAGE
# =========================

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Configure ASP.NET Core
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose container port
EXPOSE 80

# Start application
ENTRYPOINT ["dotnet", "MySampleApi.dll"]