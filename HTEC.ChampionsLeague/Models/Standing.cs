using System.ComponentModel.DataAnnotations.Schema;

namespace HTEC.ChampionsLeague
{
    public class Standing : Entity
    {
        public Standing() {}

        public Standing(string team, int homeTeamPts, int awayTeamPts, int homeTeamGoals, int awayTeamGoals, int tableId, bool homeTeam)
        {
            Team = team;
            PlayedGames = 1;
            Points = homeTeam ? homeTeamPts : awayTeamPts;
            Goals = homeTeam ? homeTeamGoals : awayTeamGoals;
            GoalsAgainst = homeTeam ? awayTeamGoals : homeTeamGoals;
            GoalDifference = homeTeam ?  homeTeamGoals - awayTeamGoals : awayTeamGoals - homeTeamGoals;
            Win = homeTeam ? homeTeamPts == 3 ? 1 : 0 : awayTeamPts == 3 ? 1 : 0;
            Lose = homeTeam ? homeTeamPts == 0 ? 1 : 0 : awayTeamPts == 0 ? 1 : 0;
            Draw = homeTeam ? homeTeamPts == 1 ? 1 : 0 : awayTeamPts == 1 ? 1 : 0;
            TableId = tableId;
        }

        public string Team { get; set; }

        public int PlayedGames { get; set; }

        public int Points { get; set; }

        public int Goals { get; set; }

        public int GoalsAgainst { get; set; }

        public int GoalDifference { get; set; }

        public int Win { get; set; }

        public int Lose { get; set; }

        public int Draw { get; set; }

        public int TableId { get; set; }

        [ForeignKey("TableId")]
        public Table Table { get; set; }
    }
}
