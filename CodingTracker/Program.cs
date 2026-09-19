using CodingTracker.Configuration;
using CodingTracker.Repositories;
using CodingTracker.Services;
using CodingTracker.UI;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var config = ConfigurationLoader.Load();
AppConfig.Initialize(config);

DatabaseInitService database = new();
database.CreateDirectory();
database.InitTable(config.ConnectionString);

MainMenu.Show();


