# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app


COPY RubberProductionManagement.csproj ./
RUN dotnet restore RubberProductionManagement.csproj

COPY . ./
RUN dotnet publish RubberProductionManagement.csproj -c Release -o out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published app
COPY --from=build /app/out ./

# Copy SSL certificate
COPY aspnetapp.pfx /https/aspnetapp.pfx

# Expose HTTPS port
EXPOSE 5001

# Run app
ENTRYPOINT ["dotnet", "RubberProductionManagement.dll"]
