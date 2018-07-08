using HTEC.ChampionsLeague.Context;
using HTEC.ChampionsLeague.Dto;
using System.Linq;

namespace HTEC.ChampionsLeague.Services
{
    public class StandingService : IStandingService
    {
        private readonly ApplicationContext _db;

        public StandingService(ApplicationContext db)
        {
            _db = db;
        }

        public void AddOrUpdateStanding(Table table, string team, PtsAndGoalsDto ptsAndGoals, bool homeTeam)
        {
            var standing = table.Standing.SingleOrDefault(x => x.Team == team);
            if (standing == null)
            {
                standing = new Standing(team,
                    ptsAndGoals.HomeTeamPts,
                    ptsAndGoals.AwayTeamPts,
                    ptsAndGoals.HomeTeamGoals,
                    ptsAndGoals.AwayTeamGoals, table.Id, homeTeam);
                _db.Standings.Add(standing);
            }
            else
            {
                standing.PlayedGames += 1;
                standing.Points += homeTeam ? ptsAndGoals.HomeTeamPts : ptsAndGoals.AwayTeamPts;
                standing.Goals += homeTeam ? ptsAndGoals.HomeTeamGoals : ptsAndGoals.AwayTeamGoals;
                standing.GoalsAgainst += homeTeam ? ptsAndGoals.AwayTeamGoals : ptsAndGoals.HomeTeamGoals;
                standing.GoalDifference = standing.Goals - standing.GoalsAgainst;
                standing.Win += homeTeam ? ptsAndGoals.HomeTeamPts == 3 ? 1 : 0 : ptsAndGoals.AwayTeamPts == 3 ? 1 : 0;
                standing.Lose += homeTeam ? ptsAndGoals.HomeTeamPts == 0 ? 1 : 0 : ptsAndGoals.AwayTeamPts == 0 ? 1 : 0;
                standing.Draw += homeTeam ? ptsAndGoals.HomeTeamPts == 1 ? 1 : 0 : ptsAndGoals.AwayTeamPts == 1 ? 1 : 0;
            }
        }

        public void RemoveOrUpdateStanding(Table table, string team, PtsAndGoalsDto ptsAndGoals, bool homeTeam)
        {
            var standing = table.Standing.Single(x => x.Team == team);
            if (standing.PlayedGames == 1)
            {
                _db.Standings.Remove(standing);
            }
            else
            {
                standing.PlayedGames -= 1;
                standing.Points -= homeTeam ? ptsAndGoals.HomeTeamPts : ptsAndGoals.AwayTeamPts;
                standing.Goals -= homeTeam ? ptsAndGoals.HomeTeamGoals : ptsAndGoals.AwayTeamGoals;
                standing.GoalsAgainst -= homeTeam ? ptsAndGoals.AwayTeamGoals : ptsAndGoals.HomeTeamGoals;
                standing.GoalDifference = standing.Goals - standing.GoalsAgainst;
                standing.Win -= homeTeam ? ptsAndGoals.HomeTeamPts == 3 ? 1 : 0 : ptsAndGoals.AwayTeamPts == 3 ? 1 : 0;
                standing.Lose -= homeTeam ? ptsAndGoals.HomeTeamPts == 0 ? 1 : 0 : ptsAndGoals.AwayTeamPts == 0 ? 1 : 0;
                standing.Draw -= homeTeam ? ptsAndGoals.HomeTeamPts == 1 ? 1 : 0 : ptsAndGoals.AwayTeamPts == 1 ? 1 : 0;
            }
        }
    }
}
