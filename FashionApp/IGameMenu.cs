using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaGame.FashionApp
{
    public interface IGameMenu
    {
        void StartGame();
        void NewGame();
        void LoadGame();
        void CampaignMode();
        void Credits();
        void ExitGame();
    }
}

    