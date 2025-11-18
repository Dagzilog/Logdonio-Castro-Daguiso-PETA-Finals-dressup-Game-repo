using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaGame.FashionApp
{
    public abstract class GameBase : IGameMenu
    {

        protected string playerName;

        public GameBase(string name)
        {
            playerName = name;
            Console.WriteLine($"Welcome to our fashion dress change game, {playerName}!");
        }

        public abstract void ShowMenu();
        public abstract void NewGame();
        public abstract void CampaignMode();
        public abstract void Picking();
        public virtual void Credits()
        {
            Console.WriteLine("--- Fashion Dress Up Game ---");
            Console.WriteLine("--- Game Developed By ---");
            Thread.Sleep(800);
            Console.WriteLine("The Koolpals Developers");
            Thread.Sleep(800);
            Console.WriteLine("1. Romel Louis S. Daguiso    (Developer/Programist");
            Thread.Sleep(800);
            Console.WriteLine("Christian Warren Castro      (Document/Paperist)");
            Thread.Sleep(800);
            Console.WriteLine("Dexter Logdonio      (Document/Paperist)");
            Thread.Sleep(800);
            Console.WriteLine("Thank you for playing our game!");
            Thread.Sleep(800);
            Console.WriteLine("Held and taught by Sir Afan Lorenz Christopher");
            Console.WriteLine("-----------------------------------------------");
        }
        public abstract void Exit();
    }

}