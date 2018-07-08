using System.Collections.Generic;

namespace HTEC.ChampionsLeague
{
    public class Table : Entity
    {
        public string LeagueTitle { get; set; }

        public int Matchday { get; set; }

        public string Group { get; set; }

        public List<Standing> Standing { get; set; }
    }
}
