using StevenClausson.HabitTracker;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Runtime.CompilerServices;
using System.Data.SQLite;

DatabaseHelper.InitializeDatabase();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Welcome to Your Habit Tracker!");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("Created by Steven Clausson!");
var date = DateTime.Now;
Console.WriteLine($"The current date and time is {date}.");
Console.WriteLine("Press any key to start!");
Console.ReadLine();
UserInput.GetUserInput();
