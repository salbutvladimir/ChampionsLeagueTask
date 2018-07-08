using System;

namespace HTEC.ChampionsLeague.Utils.Helpers
{
    public class MatchHelper
    {
        public static int GetNumberOfHomeTeamPoints(string matchResult)
        {
            var result = matchResult.Split(':');
            if (result[0] != result[1])
            {
                return Convert.ToInt32(result[0]) > Convert.ToInt32(result[1]) ? 3 : 0;
            }
            return 1;
        }

        public static int GetNumberOfGoals(string matchResult, bool homeTeam)
        {
            var result = matchResult.Split(':');

            return homeTeam ? Convert.ToInt32(result[0]) : Convert.ToInt32(result[1]);
        }
    }
}
