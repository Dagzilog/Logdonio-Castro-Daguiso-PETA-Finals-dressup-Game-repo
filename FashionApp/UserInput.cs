using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaGame.FashionApp
{
    public interface UserInput
    {
        void SetOption(string key, string value);
        string GetOption(string key);
    }
}
