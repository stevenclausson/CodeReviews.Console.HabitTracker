using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StevenClausson.HabitTracker
{
    public class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
    public class UserInput
    {

        public static void GetUserInput()
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
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        InsertRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
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
            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid Number!\n\n");
                numberInput = Console.ReadLine();
            }
            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
        static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert date dd-mm-yy. Type 0 to return to main menu.");
            string dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                GetUserInput();
            }
            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid Date!\n\n");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
        static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SQLiteConnection(DatabaseHelper.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM drinking_water ";
                List<DrinkingWater> tableData = new();
                SQLiteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            }); ;
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                connection.Close();

                Console.WriteLine("----------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");

                }
                Console.WriteLine("-----------------------------------------------------\n");
            }
        }
        static void DeleteRecord()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type ID of record you want to delete.\n\n");

            using (var connection = new SQLiteConnection(DatabaseHelper.connectionString)) 
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from drinking_water WHERE id = '{recordId}'";
                int rowCount = tableCmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    Console.ReadLine();
                    DeleteRecord();
                }
            }
            Console.WriteLine($"\n\nRecord with ID {recordId} was deleted. \n\n");
        }

        static void UpdateRecord()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type Id of the record you would like to update.");

            using (var connection = new SQLiteConnection(DatabaseHelper.connectionString))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    Console.ReadLine();
                    connection.Close();
                    UpdateRecord();
                }

                string date = GetDateInput();
                int quantity = GetNumberInput("\n\nPlease insert number of glasses.\n\n");
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE id = {recordId}";
                //*****MAKE SURE YOU EXECUTE YOUR QUERY***********************
                tableCmd.ExecuteNonQuery();
                Console.WriteLine("\nUpdated!");
                connection.Close();
            }
        }
    }
}
