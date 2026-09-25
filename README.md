# Coding Tracker

Console based CRUD application to track coding sessions.
Developed using C#, SQLite, Dapper and Spectre.Console.

# Features

- Create, view, update and delete coding sessions.
- Enter start and end time manually or use a stopwatch.
- Calculate session duration.
- Sort sessions by duration.
- Select multiple sessions to delete.
- Store records in a SQLite database.

# Challenges

- Working with Dapper and SQL queries.
- Creating interactive menus with Spectre.Console.
- Separating commands, database logic and UI.

# How to Use

Run the application using `dotnet run` from the project folder.
Use arrow keys to navigate the menu and Enter to select an action.

- **Start new session** — press any key to start the stopwatch, then press any key to stop and save the session.
- **Add new session** — enter start and end time in `dd-MM-yy HH:mm:ss` format, for example `25-09-26 14:30:00`. Duration is calculated automatically.
- **View sessions** — view all records and sort them by duration in ascending or descending order.
- **Update session** — enter the session Id, then enter new start and end time.
- **Delete session** — select sessions with Space and press Enter to delete them. Press Enter without selecting anything to cancel.
- **Exit** — close the application.

Records are saved in SQLite and remain available after restarting the application.

# Areas to Improve

- Add filtering by day, week and year.
- Improve input validation.
- Add more tests.