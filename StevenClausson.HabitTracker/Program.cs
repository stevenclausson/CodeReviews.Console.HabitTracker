using StevenClausson.HabitTracker;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Runtime.CompilerServices;
using System.Data.SQLite;

DatabaseHelper.InitializeDatabase();
GetUserInput();

static void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while (closeApp == false)
    {
        Console.WriteLine("\n\nMain Menu:");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to EXIT application");
        Console.WriteLine("Type 1 to VIEW ALL records");
        Console.WriteLine("Type 2 to INSERT record");
        Console.WriteLine("Type 3 to DELETE record");
        Console.WriteLine("Type 4 to UPDATE record");
        Console.WriteLine("-----------------------------------------------\n");

        string command = Console.ReadLine();
        switch (command)
        {
            case "0":
                Console.WriteLine("\nGoodbye!\n");
                closeApp = true;
                break;

            //case 1:
            //    GetAllRecords();
            //    break;
            case "2":
                InsertRecord();
                break;
            //case 3:
            //    DeleteRecord();
            //    break;
            //case 4:
            //    UpdateRecord();
            //    break;
            default:
                Console.WriteLine("\nInvalid. Please select a number from 0 to 4.\n");
                break;
        }
    }
}
static void InsertRecord()
{
    string date = GetDateInput();
    int quantity = GetNumberInput("\n\nPlease insert number of glasses.\n\n");

    using (var connection = new SQLiteConnection(DatabaseHelper.connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}

static int GetNumberInput(string message)
{
    Console.WriteLine(message);
    string numberInput = Console.ReadLine();
    if (numberInput == "0")
    {
        GetUserInput();
    }
    int finalInput = Convert.ToInt32(numberInput);
    return finalInput;
}
static string GetDateInput()
{
    Console.WriteLine("\n\nPlease insert date dd-mm-yy. Type 0 to return to main menu.");
    string dateInput = Console.ReadLine();

    if(dateInput == "0")
    {
        GetUserInput();
    }
    return dateInput;
}