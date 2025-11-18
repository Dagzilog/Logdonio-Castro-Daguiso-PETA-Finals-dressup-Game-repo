using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaGame.FashionApp
{
    
    public abstract class GameBase : IGameMenu
    {
        protected string connectionString;

        public GameBase(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public abstract void StartGame();
        public abstract void NewGame();
        public abstract void LoadGame();
        public abstract void CampaignMode();

        public virtual void Credits()
        {
            Console.WriteLine("--- Fashion Dress Up Game ---");
            Console.WriteLine("--- Game Developed By ---");
            Thread.Sleep(800);
            Console.WriteLine("The Koolpals Developers");
            Thread.Sleep(800);
            Console.WriteLine("1. Romel Louis S. Daguiso    (Developer/Programist)");
            Thread.Sleep(800);
            Console.WriteLine("2.Christian Warren Castro      (Document/Paperist)");
            Thread.Sleep(800);
            Console.WriteLine("3.Dexter Logdonio      (Document/Paperist/Leader)");
            Thread.Sleep(800);
            Console.WriteLine("Thank you for playing our game!");
            Thread.Sleep(800);
            Console.WriteLine("Held and taught by  Sir Afan Lorenz Christopher\n18/11/25 Dagz");
            Console.WriteLine("-----------------------------------------------");
        }
        public void ExitGame()
        {
            Console.WriteLine("Exiting game. Goodbye!");
            Credits();
            Environment.Exit(0);
        }
    }

}