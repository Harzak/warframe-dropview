# Warframe DropView

Visual explorer for Warframe drop tables.
Personal project used to practice front-end development.

## Architecture

The solution consists of:

- **Backend.API**: ASP.NET Core minimal API exposing search endpoints
- **Backend.DropTableParser**: Console application to populate MongoDB from Warframe HTML drop tables
- **Backend.Models**: Shared data models
- **Backend.Abstractions**: Interfaces and result types
- **Backend.Plugin.MongoDB**: MongoDB repository implementations

## Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/Harzak/warframe-dropview.git
cd warframe-dropview
```

### 2. Start the Containers

Run the API and MongoDB in containers:

```bash
docker compose up -d
```

This will:
- Start MongoDB on `localhost:27017` with a persistent volume
- Build and start the API on `http://localhost:8080`
- Wait for MongoDB to be healthy before starting the API

Verify the containers are running:

```bash
docker compose ps
```

You should see:
- `warframe-dropview-api` - healthy
- `warframe-dropview-mongodb` - healthy

### 3. Populate the Database

The database starts empty. You need to run the **DropTableParser** to load drop data.

#### Run Locally

```bash
cd backend/warframe-dropview.Backend.DropTableParser
dotnet run
```

The parser will:
1. Connect to MongoDB running in the container (`localhost:27017`)
2. Parse the HTML file from `Resources/Warframe-PC-Drops_2025_10_21.html`
3. Insert mission drops, relic drops, and enemy drops into the database

### 5. Stop the Containers

```bash
docker compose down
```

To also remove the MongoDB volume (deletes all data):

```bash
docker compose down -v
```

## License

[MIT License](LICENSE)