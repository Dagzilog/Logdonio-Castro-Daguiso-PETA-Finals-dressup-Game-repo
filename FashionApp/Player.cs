using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaGame.FashionApp
{
    class Player : UserInput
    {
        // Ensure Name is initialized to avoid converting null to non-nullable
        public string Name { get; set; } = string.Empty;

        private Dictionary<string, string> selections = new Dictionary<string, string>();

        public void SetOption(string key, string value)
        {
            if (selections.ContainsKey(key))
                selections[key] = value;
            else
                selections.Add(key, value);
        }

        public string GetOption(string key)
        {
            return selections.ContainsKey(key) ? selections[key] : "";
        }

        public Dictionary<string, string> GetAllSelections()
        {
            return selections;
        }
    }
}
