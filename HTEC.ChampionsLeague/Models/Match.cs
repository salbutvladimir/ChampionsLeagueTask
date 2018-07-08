using System;

namespace HTEC.ChampionsLeague
{
    public class Match : Entity
    {
        public string LeagueTitle { get; set; }

        public int Matchday { get; set; }

        public string Group { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public DateTime KickoffAt { get; set; }

        public string Score { get; set; }
    }
}
