using Microsoft.Extensions.Configuration;

namespace Coding.Tracker
{
    public static class Program
    {
        private static CodeSessionService service;
        public static void Main(string[] args)
        {
            if (!Configurate()) return;
            service.CreateDatabase();
            while (true)
            {
                try
                {
                    SpectreConsoleUI.Clear();
                    var choice = ActionExecuter.GetUserAction();
                    if (choice == UserAction.Exit) 
                        break;
                    ActionExecuter.Execute(choice, service);
                }
                catch (Exception ex)
                {
                    SpectreConsoleUI.PrintMessage($"{ex.Message}", "red");
                    Console.ReadKey();
                }
            }
        }

        private static bool Configurate()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                service = new CodeSessionService(connectionString);
                return true;
            }
            catch
            {
                SpectreConsoleUI.PrintMessage("Unable to load configuration");
                return false;
            }
        }
    }
}
