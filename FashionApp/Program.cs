using Microsoft.Data.SqlClient;
using PetaGame.FashionApp;
using System;
using System.IO;

namespace fashionDressChangeSpace
{
    class fashionAppMain
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            string? playerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName)) playerName = "Player";

            // Absolute path to your database
            string mdfPath = @"C:\Users\romel\source\repos\PetaGame\PetaGame\FashionDataBase_Fresh.mdf";

            if (!File.Exists(mdfPath))
            {
                Console.WriteLine($"Database file not found at: {mdfPath}");
                return;
            }

            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True;Connect Timeout=30";

            try
            {
                // Pass playerName directly so it doesn't ask again
                FashionGame game = new FashionGame(connectionString, playerName);
                game.StartGame();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}