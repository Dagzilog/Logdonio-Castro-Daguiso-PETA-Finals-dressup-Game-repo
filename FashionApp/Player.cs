using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaGame.FashionApp
{
    class Player : UserInput
    {
        private Dictionary<string, string> options = new Dictionary<string, string>();

        public void SetOption(string key, string value)
        {
            options[key] = value;
        }

        public string GetOption(string key)
        {
            return options.ContainsKey(key) ? options[key] : "Not Selected";
        }

        public void ShowSelections()
        {
            Console.WriteLine("\n--- Your Selections ---");
            foreach (var kvp in options)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine("----------------------\n");
        }
    }
}
