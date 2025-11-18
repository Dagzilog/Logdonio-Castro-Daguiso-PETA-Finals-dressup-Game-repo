using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PetaGame.FashionApp
{
    public class FashionGame : GameBase
    {
        // initialize player so field is never null
        private Player player = new Player();

        // accept optional playerName passed from Program; keeps backwards-compat if null
        public FashionGame(string dbConnectionString, string? playerName = null) : base(dbConnectionString)
        {
            Console.WriteLine("Welcome to the Fashion Dress Change Game!");

            // only set player name; do NOT ask again
            player.Name = string.IsNullOrWhiteSpace(playerName) ? "Player" : playerName.Trim();

            Console.WriteLine($"\nHello {player.Name}, enjoy the game!\n");
        }

        public override void StartGame()
        {
            while (true)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. New Game");
                Console.WriteLine("2. Load Game");
                Console.WriteLine("3. Campaign Mode");
                Console.WriteLine("4. Credits");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine();

                switch (choice?.Trim())
                {
                    case "1":
                        NewGame();
                        break;
                    case "2":
                        LoadGame();
                        break;
                    case "3":
                        CampaignMode();
                        break;
                    case "4":
                        Credits();
                        break;
                    case "5":
                        ExitGame();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        // ------------------ New Game ------------------
        public override void NewGame()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Console.WriteLine("\n--- New Game Picking ---");
                    PickOptions(conn);
                    SavePlayerHistory(conn);

                    Console.WriteLine("\n--- Your Selection ---");
                    foreach (var kvp in player.GetAllSelections())
                    {
                        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // ------------------ Campaign Mode ------------------
        public override void CampaignMode()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Console.WriteLine("\n--- Campaign Mode ---");

                    PickOptions(conn); // Let player pick outfit

                    // Random message
                    string[] messages = {
                        "You look stunning today!",
                        "Your style is unmatched!",
                        "Everyone is admiring your outfit!",
                        "What a fashion icon!"
                    };

                    Random rnd = new Random();
                    int index = rnd.Next(messages.Length);
                    Console.WriteLine("\n" + messages[index]);

                    Console.WriteLine("\n--- Your Selection ---");
                    foreach (var kvp in player.GetAllSelections())
                    {
                        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // ------------------ Load Game ------------------
        public override void LoadGame()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM PlayerHistory WHERE PlayerName=@PlayerName";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PlayerName", player.Name);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\n--- Past Selections ---");
                            while (reader.Read())
                            {
                                Console.WriteLine($"Date: {reader["DatePlayed"]}");
                                Console.WriteLine($"Gender: {reader["Gender"]}");
                                Console.WriteLine($"Hair: {reader["HairType"]} / {reader["HairBraided"]} / {reader["HairNormal"]}");
                                Console.WriteLine($"Face: {reader["FaceShape"]}");
                                Console.WriteLine($"Eyes: {reader["EyeColor"]}");
                                Console.WriteLine($"Skin: {reader["SkinTone"]}");
                                Console.WriteLine($"Body: {reader["BodyType"]}");
                                Console.WriteLine($"TopDress: {reader["TopDress"]}");
                                Console.WriteLine($"Accessories: {reader["Accessories"]}");
                                Console.WriteLine($"Shoes: {reader["Shoes"]} ({reader["ShoeColor"]})");
                                Console.WriteLine($"Pose: {reader["Pose"]}, VideoMode: {reader["VideoMode"]}");
                                Console.WriteLine($"Background: {reader["Background"]}, Pet: {reader["Pet"]}, Walk: {reader["WalkAnimation"]}");
                                Console.WriteLine("-----------------------------");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading history: " + ex.Message);
            }
        }
        // ------------------ Picking Options ------------------
        private void PickOptions(SqlConnection conn)
        {
            // 1. Gender first
            string gender = GetOptionFromDB(conn, "GenderOptions");
            player.SetOption("Gender", gender);

            // 2. Hair Type
            Console.WriteLine("Choose Hair Type: 1.Normal 2.Braided");
            string hairChoice = ReadNonEmptyString("Enter choice: ");
            player.SetOption("HairType", hairChoice == "2" ? "Braided" : "Normal");

            if (hairChoice == "2")
                player.SetOption("HairBraided", GetOptionFromDB(conn, "HairBraidedOptions"));
            else
                player.SetOption("HairNormal", GetOptionFromDB(conn, "HairNormalOptions"));

            // 3. Hair Color
            player.SetOption("HairColor", GetOptionFromDB(conn, "HairColorOptions"));

            // 4. Face Shape
            player.SetOption("FaceShape", GetOptionFromDB(conn, "FaceShapeOptions"));

            // 5. Eye Color
            player.SetOption("EyeColor", GetOptionFromDB(conn, "EyeColorOptions"));

            // 6. Skin Tone
            player.SetOption("SkinTone", GetOptionFromDB(conn, "BodySkinToneOptions"));

            // 7. Body Type
            player.SetOption("BodyType", GetOptionFromDB(conn, "BodyTypeOptions"));

            // 8. Top Dress
            player.SetOption("TopDress", GetOptionFromDB(conn, "TopDressOptions"));

            // 9. Accessories
            HandleAccessories(conn);

            // 10. Shoes and color
            player.SetOption("Shoes", GetOptionFromDB(conn, "ShoeOptions"));
            player.SetOption("ShoeColor", GetOptionFromDB(conn, "ShoeColorOptions"));

            // 11. Pose, Video, Background, Pet, WalkAnimation
            player.SetOption("Pose", GetOptionFromDB(conn, "PoseOptions"));
            player.SetOption("VideoMode", GetOptionFromDB(conn, "VideoModeOptions"));
            player.SetOption("Background", GetOptionFromDB(conn, "BackgroundOptions"));
            player.SetOption("Pet", GetOptionFromDB(conn, "CityPetOptions"));
            player.SetOption("WalkAnimation", GetOptionFromDB(conn, "WalkAnimationOptions"));
        }

        // ------------------ Accessories ------------------
        private void HandleAccessories(SqlConnection conn)
        {
            Console.Write("How many earrings? ");
            int eCount = ReadIntFromConsole();
            for (int i = 1; i <= eCount; i++)
            {
                player.SetOption("Earring" + i, GetOptionFromDB(conn, "EarringOptions"));
            }

            Console.Write("How many necklaces? ");
            int nCount = ReadIntFromConsole();
            for (int i = 1; i <= nCount; i++)
            {
                player.SetOption("Necklace" + i, GetOptionFromDB(conn, "NecklaceOptions"));
            }

            Console.Write("How many bracelets? ");
            int bCount = ReadIntFromConsole();
            for (int i = 1; i <= bCount; i++)
            {
                player.SetOption("Bracelet" + i, GetOptionFromDB(conn, "BraceletOptions"));
            }

            Console.Write("How many rings? ");
            int rCount = ReadIntFromConsole();
            for (int i = 1; i <= rCount; i++)
            {
                player.SetOption("Ring" + i, GetOptionFromDB(conn, "RingOptions"));
            }
        }

        // ------------------ Save History ------------------
        private void SavePlayerHistory(SqlConnection conn)
        {
            string accessories = "";
            foreach (var kvp in player.GetAllSelections())
            {
                if (kvp.Key.Contains("Earring") || kvp.Key.Contains("Necklace") ||
                    kvp.Key.Contains("Bracelet") || kvp.Key.Contains("Ring"))
                    accessories += kvp.Key + ":" + kvp.Value + "; ";
            }

            string query = @"INSERT INTO PlayerHistory
                (PlayerName, Gender, HairType, HairBraided, HairNormal, HairColor, FaceShape, EyeColor,
                 SkinTone, BodyType, TopDress, Accessories, Shoes, ShoeColor, Pose, VideoMode, Background, Pet, WalkAnimation)
                 VALUES
                (@PlayerName, @Gender, @HairType, @HairBraided, @HairNormal, @HairColor, @FaceShape, @EyeColor,
                 @SkinTone, @BodyType, @TopDress, @Accessories, @Shoes, @ShoeColor, @Pose, @VideoMode, @Background, @Pet, @WalkAnimation)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PlayerName", player.Name);
                cmd.Parameters.AddWithValue("@Gender", player.GetOption("Gender"));
                cmd.Parameters.AddWithValue("@HairType", player.GetOption("HairType"));
                cmd.Parameters.AddWithValue("@HairBraided", player.GetOption("HairBraided"));
                cmd.Parameters.AddWithValue("@HairNormal", player.GetOption("HairNormal"));
                cmd.Parameters.AddWithValue("@HairColor", player.GetOption("HairColor"));
                cmd.Parameters.AddWithValue("@FaceShape", player.GetOption("FaceShape"));
                cmd.Parameters.AddWithValue("@EyeColor", player.GetOption("EyeColor"));
                cmd.Parameters.AddWithValue("@SkinTone", player.GetOption("SkinTone"));
                cmd.Parameters.AddWithValue("@BodyType", player.GetOption("BodyType"));
                cmd.Parameters.AddWithValue("@TopDress", player.GetOption("TopDress"));
                cmd.Parameters.AddWithValue("@Accessories", accessories);
                cmd.Parameters.AddWithValue("@Shoes", player.GetOption("Shoes"));
                cmd.Parameters.AddWithValue("@ShoeColor", player.GetOption("ShoeColor"));
                cmd.Parameters.AddWithValue("@Pose", player.GetOption("Pose"));
                cmd.Parameters.AddWithValue("@VideoMode", player.GetOption("VideoMode"));
                cmd.Parameters.AddWithValue("@Background", player.GetOption("Background"));
                cmd.Parameters.AddWithValue("@Pet", player.GetOption("Pet"));
                cmd.Parameters.AddWithValue("@WalkAnimation", player.GetOption("WalkAnimation"));

                cmd.ExecuteNonQuery();
            }
        }

        // ------------------ Get Option from DB ------------------
        private string GetOptionFromDB(SqlConnection conn, string tableName)
        {
            Console.WriteLine($"\nSelect {tableName}:");
            string query = $"SELECT * FROM {tableName}"; // avoid duplicate column lists
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                // print by name if available, fall back to ordinal 0/1 safe checks
                while (reader.Read())
                {
                    string display;
                    if (reader.FieldCount > 1 && !reader.IsDBNull(1))
                        display = reader.GetValue(1)?.ToString() ?? string.Empty;
                    else if (reader.FieldCount > 0 && !reader.IsDBNull(0))
                        display = reader.GetValue(0)?.ToString() ?? string.Empty;
                    else
                        display = string.Empty;

                    Console.WriteLine($"{reader["Id"]}. {display}");
                }
            }

            int choiceId = ReadIntFromConsole("Enter choice number: ");

            string selected = "";

            using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {tableName} WHERE Id=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", choiceId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        selected = (reader.FieldCount > 1 && !reader.IsDBNull(1))
                            ? reader.GetValue(1)?.ToString() ?? string.Empty
                            : (reader.FieldCount > 0 && !reader.IsDBNull(0)) ? reader.GetValue(0)?.ToString() ?? string.Empty : string.Empty;
                    }
                }
            }
            return selected;
        }

        // Helper: read an integer safely from console
        private int ReadIntFromConsole(string? prompt = null)
        {
            while (true)
            {
                if (!string.IsNullOrEmpty(prompt))
                    Console.Write(prompt);

                string? input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                    return result;

                Console.WriteLine("Invalid number. Please enter a valid integer.");
            }
        }

        // Helper: read a non-empty string (returns empty string only if user keeps entering whitespace)
        private string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Input cannot be empty. Please try again.");
            }
        }
    }
}