
public class Input
{
    public static void GetUserInput()
    {

        bool end = false;
        while (end == false)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the coding tracker!\n");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Press 0 to close the console.");
            Console.WriteLine("Press 1 to view Records.");
            Console.WriteLine("Press 2 to Insert a Record.");
            Console.WriteLine("Press 3 to Delete a Record.");
            Console.WriteLine("Press 4 to Update a Record.");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    end = true;
                    Console.WriteLine("Goodbye\nPress any key to quit.");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;

                case "1":
                    CodingController.ViewRecords();
                    break;

                case "2":
                    InsertInput();
                    break;

                case "3":
                    DeleteInput();
                    break;

                case "4":
                    UpdateInput();
                    break;

                default:
                    Console.WriteLine("Please enter an option between 1 and 4.");
                    break;
            }
        }
    }

    public static DateTime? GeTimeInput(string message)
    {
        Console.WriteLine(message);
        string getTime = Console.ReadLine();

        return Validation.ValidateTime(getTime);

    }

    public static void InsertInput()
    {
        Console.Clear();

        DateTime? startTime = GeTimeInput("Please enter the start Time Format: (dd-MM-yyyy, HH:mm), Or enter 0 to exit.");
        if (startTime == null) return;

        DateTime? endTime = GeTimeInput("Please enter the end Time Format: (dd-MM-yyyy, HH:mm), Or enter 0 to exit.");
        if (endTime == null) return;


        CodingController.Insert(startTime.Value, endTime.Value);
    }

    public static void DeleteInput()
    {
        Console.Clear();
        CodingController.GetAllRecords();

        Console.WriteLine("Please enter a record ID to delete.\n\nOr enter 0 to exit");


        int id = Validation.ValidateId(Console.ReadLine());
        if (id == 0) return;

        CodingController.Delete(id);
    }

    public static void UpdateInput()
    {
        Console.Clear();
        CodingController.GetAllRecords();

        Console.WriteLine("Please type the Id of the record you want to update, or enter 0 to return to the main menu.");
        int ID = Validation.ValidateId(Console.ReadLine());
        if (ID == 0) return;

        Console.WriteLine("Please enter the new start time (dd-MM-yyyy, HH:mm), or enter 0 to return to the main menu.");
        DateTime? startTime = Validation.ValidateTime(Console.ReadLine());
        if (startTime == null) return;

        Console.WriteLine("Please enter the new end time (dd-MM-yyyy, HH:mm), or enter 0 to return to the main menu.");
        DateTime? endTime = Validation.ValidateEndTime(startTime.Value);
        if (endTime == null) return;

        string duration = (endTime.Value - startTime.Value).ToString();

        CodingController.Update(ID, startTime.Value, endTime.Value, duration);
    }
}