using HTEC.ChampionsLeague.Dto;

namespace HTEC.ChampionsLeague.Services
{
    public interface IStandingService
    {
        void AddOrUpdateStanding(Table table, string team, PtsAndGoalsDto ptsAndGoals, bool homeTeam);

        void RemoveOrUpdateStanding(Table table, string team, PtsAndGoalsDto ptsAndGoals, bool homeTeam);
    }
}
