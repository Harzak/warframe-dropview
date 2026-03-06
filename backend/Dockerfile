# ── Stage 1: Build ────────────────────────────────────────────────────────────
# Full SDK image — contains the compiler and build tools.
# Discarded after this stage; never shipped in the final image.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project manifests first so Docker can cache the restore layer.
# dotnet restore is only re-run when a .csproj or the .sln changes.
COPY warframe-dropview.Backend.sln ./
COPY warframe-dropview.Backend.Abstractions/warframe-dropview.Backend.Abstractions.csproj \
     warframe-dropview.Backend.Abstractions/
COPY warframe-dropview.Backend.Models/warframe-dropview.Backend.Models.csproj \
     warframe-dropview.Backend.Models/
COPY warframe-dropview.Backend.Repository.MongoDB/warframe-dropview.Backend.Plugin.MongoDB.csproj \
     warframe-dropview.Backend.Repository.MongoDB/
COPY warframe-dropview.Backend.API/warframe-dropview.Backend.API.csproj \
     warframe-dropview.Backend.API/
COPY warframe-dropview.Backend.DropTableParser/warframe-dropview.Backend.DropTableParser.csproj \
     warframe-dropview.Backend.DropTableParser/

RUN dotnet restore warframe-dropview.Backend.sln

# Copy the rest of the source and publish the API in Release mode.
COPY . .
RUN dotnet publish warframe-dropview.Backend.API/warframe-dropview.Backend.API.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

# ── Stage 2: Runtime ──────────────────────────────────────────────────────────
# Lightweight ASP.NET runtime image — no SDK, much smaller surface area.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# ASP.NET Core defaults to port 8080 in non-Development environments.
EXPOSE 8080

ENTRYPOINT ["dotnet", "warframe-dropview.Backend.API.dll"]
