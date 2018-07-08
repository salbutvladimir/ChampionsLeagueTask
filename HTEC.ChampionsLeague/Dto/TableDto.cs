using System.Collections.Generic;

namespace HTEC.ChampionsLeague.Dto
{
    public class TableDto
    {
        public string LeagueTitle { get; set; }

        public int Matchday { get; set; }

        public string Group { get; set; }

        public List<StandingDto> Standing { get; set; }
    }
}
