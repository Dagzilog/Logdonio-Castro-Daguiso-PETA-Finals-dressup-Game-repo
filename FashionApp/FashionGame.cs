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
        private Player player;
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\romel\source\repos\PetaGame\PetaGame\FashionDataBase_fresh.mdf;Integrated Security=True;Connect Timeout=30";

        public FashionGame(string name) : base(name)
        {
            player = new Player();
        }

        // -------------------- MENU --------------------
        public override void ShowMenu()
        {
            bool repeat = true;
            while (repeat)
            {
                Console.WriteLine("\n1. New Game\n2. Load Game\n3. Campaign Mode\n4. Credits\n5. Exit");
                Console.Write("Choose an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": NewGame(); break;
                    case "2": Console.WriteLine("Load game not implemented."); break;
                    case "3": CampaignMode(); break;
                    case "4": Credits(); break;
                    case "5": Exit(); repeat = false; break;
                    default: Console.WriteLine("Invalid option!"); break;
                }
            }
        }

        // -------------------- NEW GAME --------------------
        public override void NewGame()
        {
            Console.WriteLine("\n--- New Game Started ---");
            Picking();
            player.ShowSelections();
        }

        // -------------------- CAMPAIGN MODE --------------------
        public override void CampaignMode()
        {
            Console.WriteLine("\n--- Campaign Mode ---");
            Picking();
            player.ShowSelections();

            // Random output
            string[] randomComments = {
                "You dressed well!",
                "You look stunning today!",
                "Fashion icon alert!",
                "Wow, such style!",
                "You nailed that look!"
            };
            Random rand = new Random();
            int index = rand.Next(randomComments.Length);
            Console.WriteLine("\n" + randomComments[index]);
        }

        // -------------------- PICKING --------------------
        public override void Picking()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Example: Hair braided selection
                    Console.WriteLine("\nChoose Hair Type: 1. Normal 2. Braided");
                    string hairType = Console.ReadLine();
                    if (hairType == "2")
                    {
                        Console.WriteLine("Choose braided hairstyle:");
                        string braided = GetOptionFromDB(conn, "HairBraidedOptions");
                        player.SetOption("HairBraided", braided);
                    }
                    else
                    {
                        Console.WriteLine("Choose normal hairstyle:");
                        string normal = GetOptionFromDB(conn, "HairNormalOptions");
                        player.SetOption("HairNormal", normal);
                    }

                    // Example: Accessories
                    Console.WriteLine("How many earrings do you want?");
                    int numEarrings = int.Parse(Console.ReadLine());
                    for (int i = 0; i < numEarrings; i++)
                    {
                        string earring = GetOptionFromDB(conn, "EarringOptions");
                        player.SetOption($"Earring{i + 1}", earring);
                    }

                    Console.WriteLine("How many necklaces do you want?");
                    int numNecklaces = int.Parse(Console.ReadLine());
                    for (int i = 0; i < numNecklaces; i++)
                    {
                        string necklace = GetOptionFromDB(conn, "NecklaceOptions");
                        player.SetOption($"Necklace{i + 1}", necklace);
                    }

                    // Similarly, you can add bracelets, rings, shoes, pose, etc.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // -------------------- DATABASE HELPER --------------------
        private string GetOptionFromDB(SqlConnection conn, string table)
        {
            string option = "";
            string query = $"SELECT * FROM {table}";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                List<string> choices = new List<string>();
                while (reader.Read())
                {
                    choices.Add(reader[1].ToString()); // assume second column is the option name
                }

                Console.WriteLine($"Available options for {table}:");
                for (int i = 0; i < choices.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {choices[i]}");
                }

                Console.Write("Select option number: ");
                int choiceNum = int.Parse(Console.ReadLine());
                option = choices[choiceNum - 1];
            }

            return option;
        }

        // -------------------- EXIT --------------------
        public override void Exit()
        {
            Console.WriteLine("Thank you for playing!");
        }
    }
}
