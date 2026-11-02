![.NET Vesrion](https://img.shields.io/badge/.NET-9.0-512BD4)
![Language](https://img.shields.io/badge/Language-C%23-blue)
![Database](https://img.shields.io/badge/Database-SQLite_+_Dapper-green)

## :mag_right: About
Tracking Coding Sessions is a good way for the user to evaluate how often they code. **Coding Tracker** is a [TUI](https://en.wikipedia.org/wiki/Text-based_user_interface) application that offers simple [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete) operations to track the user's Coding Sessions by either manually typing the start and end of a coding session, or by using a built-in Stopwatch to track the session in real time. 

**Packages**:
- [Dapper](https://github.com/dapperlib/dapper)
- [ADO.NET SQLite provider](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=net-cli)
- [Spectre Console](https://spectreconsole.net/)
- [Microsoft Configuration Extensions for .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration)

## How to run locally
The app has not been published, so it needs to run locally by .NET's runtime.  
> [!IMPORTANT] 
> Make sure [**.NET SDK 9**](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) or higher is installed in order to run this project.

Clone the repository:
```
git clone https://github.com/Kunikazu723/TCSA.HabitTracker.git
cd CodeReviews.Console.CodingTracker
```

Restore dependencies:
```
dotnet restore
```

Run the project:
```
dotnet run --poject CodingTracker.Kunikazu723
```

To change databasePath, connection strings and date time format, refer to the [App Settings](#app-settings-file) section.

## 💾 How it works
First the Database is generated with 100 random Coding Sessions. Then the user is presented with 5 options tha can be selected using the keyboard's arrows, 4 of them are CRUD operations.  
Each Coding Session needs a Start Date and End Date, both are stored in the database with a default date-time format. The duration is calculated during data presentation.

## :pencil2: Architectural Choices
### Database Structure
The database has a Coding Sessions table that contains Id, Start Time and End Time. Both Start and End times follow one strict storage format (yyyy-MM-dd hh:mm:ss).  

### Data Access
Database access is made through a Data Access Object (DAO) to make client code esaily communicate with the database through simple method calls.  
Usually I would use a service class as an intermediate between the DAO and the User Interface, but for a project this size I thought it was a bit overkill. I uderstand that an intermediate would be valuable to make sure any data entering the Database is clean and follows the specified pattern. But for simplicity sake I chose to make the UI communicate directly to the DAO.  
> [!NOTE] 
> One thing worth noticing is that DateTime objects are stored in the database using Dapper calls directly without any conversion made by my code, it uses ADO.NET drivers implicit conversion that follows DateTime's ISO string format

### User Interface
As usual the UI is projected as a TUI by using [Spectre Console](https://spectreconsole.net/) package, allowing for intuitive, pretty and smooth user experience while also making it far easier to design it.

### App Settings File
The following is an example containing the required settings that the application uses to run properly.
```{
  "DatabasePath": "coding-tracker.db",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=coding-tracker.db"
  },
  "DateTimeFormat": "dd-MM-yyyy-HH-mm-ss",
  "SeedDatabase" :  true
}
```
**DatabasePath and ConnectionStrings**: Sets up the relative database path that will be outputed in de working directory. Its available, but its use is not fully fleshed out. Use Path builder and connection builder would be better but they're not currently implemented.

**DateTimeFormat**: This will define the format that will be **presented** to the user. This setting does not affect the way DateTime strings are stored into the database, it only changes presentation.

**SeedDatabase**: If set to true, the Seeding algorithm will run and the database will be populated with 100 random Coding Sessions.

## :mirror: Reflection
Even though the use of Dapper was required, it turned out to be quite simple, even simpler than using raw ADO.NET. There were few differences in the overall project structure from my [previous project](https://github.com/Kunikazu723/TCSA.HabitTracker). The only noticeable differences were the use of Dapper micro ORM, configuration files and DateTime logic. 

It was an interesting project. I wish to improve on it when I gather more experience. Maybe adding a GUI later on.

### Challenges
By far the most difficult challenge was designing a good filter for displaying Coding Sessions. Setting up the date rolling window was tricky and displayed incorrect or unexpected results. Luckily after banging my head against the wall for a bit I managed to make a predictable rolling window.

Another surprising challenge was properly accessing the [App Settings File](#app-settings-file). The task is seemingly simple but I found myself deep into the rabbit hole of configuration files and how to set them up properly. After reading some articles I found the Extension Configuration package from microsoft.