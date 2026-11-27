# Coding Tracker

Coding Tracker is a console-based application for recording and managing coding sessions. It provides a terminal menu for starting, manually adding, viewing, updating, and deleting coding sessions.

## Features

- Start a live coding session with a stopwatch
- Automatically record the session date, start time, and end time
- Manually add coding sessions
- View saved sessions in a formatted table
- Update existing sessions
- Delete sessions with confirmation
- Store session data locally using SQLite

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Setup

1. Clone or download the repository:

```bash
git clone <repository-url>
cd CodingTracker
```

2. Run the application:

```bash
dotnet run --project CodingTracker/CodingTracker.csproj
```

You can also open `CodingTracker.slnx` in Visual Studio and run the project using the start button.

## Configuration

The `appsettings.json` file contains the database path and optional sample-data settings:

```json
{
  "ConnectionStrings": {
	"DataBasePath": "Data Source = time.db"
  },
  "Seeding": {
	"Enabled": false,
	"NumberOfEntries": 10
  }
}
```

- `DataBasePath` specifies the SQLite database file.
- `Seeding:Enabled` determines whether sample sessions are added at startup.
- `Seeding:NumberOfEntries` determines how many sample sessions are generated.

Set `Enabled` to `false` for a clean database or `true` to populate it with sample entries.

## How It Works

The application starts by initializing the coding controller, loading `appsettings.json`, and creating the SQLite table if it does not already exist.

The controller displays a Spectre.Console menu. From this menu, the user can start a live session or manage previously saved sessions.

Live sessions use a stopwatch to measure elapsed time. When the stopwatch stops, the application saves the session date and its start and end times. Manually added sessions are validated before they are saved.

Each stored session includes:

- A unique session ID
- A date
- A start time
- An end time
- A calculated duration

The application uses Dapper to insert, retrieve, update, and delete session records in SQLite.