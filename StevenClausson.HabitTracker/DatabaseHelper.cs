using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace StevenClausson.HabitTracker
{
    class DatabaseHelper
    {
        public static string connectionString = @"Data Source=..\..\..\Files\HabitTracker.db;Version=3";

        public static void InitializeDatabase()
        {
            if (!File.Exists(@"..\..\..\Files\HabitTracker.db"))
            {
                SQLiteConnection.CreateFile(@"..\..\..\Files\HabitTracker.db");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    //Creates tables
                    string createWaterDrinkingTableQuery = @"
                        CREATE TABLE IF NOT EXISTS drinking_water (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            date TEXT NOT NULL,
                            quantity INTEGER NOT NULL
                            )";

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createWaterDrinkingTableQuery;
                        command.ExecuteNonQuery(); 
                    }
                }
            }
        }
    }
}
