using System.Collections.Generic;

namespace FreedomWeb.ViewModels.GameAccount
{
    public class GameAccountCharacterViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class GameAccountViewModel
    {
        public int Id { get; set;  }
        public string Name { get; set; }

        public List<GameAccountCharacterViewModel> Characters { get; set; }
    }

    public class GameAccountOverviewViewModel
    {
        public List<GameAccountViewModel> GameAccounts { get; set; }
    }
}
