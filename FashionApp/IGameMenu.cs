using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaGame.FashionApp
{
    public interface IGameMenu
    {
        void ShowMenu();
        void NewGame();
        void CampaignMode();
        void Picking();
        void Credits();
        void Exit();
    }
}

    