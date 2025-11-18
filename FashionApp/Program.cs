using Microsoft.Data.SqlClient;
using PetaGame.FashionApp;
using System;

namespace fashionDressChangeSpace
{
    class fashionAppMain
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            FashionGame game = new FashionGame(name);
            game.ShowMenu();
        }
    }
}