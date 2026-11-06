# Coding Tracker

A small .NET console application for tracking coding sessions. It uses Dapper for data access to a SQLite database, and Spectre.Console for the interactive menu.

## Features

- Add coding sessions with start and end times.
- View saved sessions and their durations.
- Update or delete a session by its ID.
- Calculate sessions that cross midnight.
- Cancel time entry by entering `0`.

## Requirements

- .NET 10 SDK

## Setup and Run

1. Clone this repository and open a terminal in the project directory.
2. In `appsettings.json`, set `ConnectionStrings:DefaultConnection` to a SQLite database location that exists on your machine. For example:

   ```json
   "DefaultConnection": "Data Source=coding-tracker.db"
   ```

   The checked-in configuration uses an absolute Windows path, so update it if that path does not exist on your machine.
3. Run the application:

   ```sh
   dotnet run
   ```

Run it in an interactive terminal so Spectre.Console can accept keyboard input. In VS Code, press **F5** and choose the `CodingTracker` launch profile.

## Menu

| Option | Action |
| --- | --- |
| `0` | Close the application |
| `1` | View all records |
| `2` | Add a record |
| `3` | Delete a record by ID |
| `4` | Update a record by ID |

Enter start and end times in 24-hour `HH:mm` format, such as `14:30`. Enter `0` at either time prompt to cancel the add or update operation and return to the menu.