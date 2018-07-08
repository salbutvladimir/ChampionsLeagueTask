using AutoMapper;
using HTEC.ChampionsLeague.Context;
using HTEC.ChampionsLeague.Dto;
using HTEC.ChampionsLeague.Utils.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTEC.ChampionsLeague.Services
{
    public class TableService : ITableService
    {
        private readonly ApplicationContext _db;
        private readonly IStandingService _standingService;

        public TableService(ApplicationContext db, IStandingService standingService)
        {
            _db = db;
            _standingService = standingService;
        }

        public async Task<List<TableDto>> GetTables(TableFilterDto filter = null)
        {
            var tableDtos = Mapper.Map<List<TableDto>>(GetTablesIQueryable(filter));

            return await CalculateRank(tableDtos);
        }

        public async Task AddOrUpdateTable(MatchDto match)
        {
            var ptsAndGoals = GetPtsAndGoals(match);

            var table = await _db.Tables.Include(x => x.Standing).SingleOrDefaultAsync(x =>
                x.LeagueTitle == match.LeagueTitle &&
                x.Group == match.Group);

            if (table == null)
            {
                InsertNewTable(match, ptsAndGoals);
            }
            else
            {
                _standingService.AddOrUpdateStanding(table, match.HomeTeam, ptsAndGoals, true);
                _standingService.AddOrUpdateStanding(table, match.AwayTeam, ptsAndGoals, false);
            }
        }

        public void RemoveOrUpdateTable(MatchDto oldMatch)
        {
            var ptsAndGoals = GetPtsAndGoals(oldMatch);

            var table = _db.Tables.Include(x => x.Standing).SingleOrDefault(x =>
                x.LeagueTitle == oldMatch.LeagueTitle &&
                x.Group == oldMatch.Group);
            
            if (table.Standing.Count == 2)
            {
                _db.Tables.Remove(table);
            }
            else
            {
                _standingService.RemoveOrUpdateStanding(table, oldMatch.HomeTeam, ptsAndGoals, true);
                _standingService.RemoveOrUpdateStanding(table, oldMatch.AwayTeam, ptsAndGoals, false);
            }
        }

        private IQueryable<Table> GetTablesIQueryable(TableFilterDto filter)
        {
            IQueryable<Table> tables = _db.Tables.Include(x => x.Standing);

            if (filter?.LeagueTitle?.Length > 0)
            {
                tables = tables.Where(x => filter.LeagueTitle.Select(s => s.ToLowerInvariant()).Contains(x.LeagueTitle.ToLowerInvariant()));
            }
            if (filter?.Group?.Length > 0)
            {
                tables = tables.Where(x => filter.Group.Select(s => s.ToLowerInvariant()).Contains(x.Group.ToLowerInvariant()));
            }

            return tables;
        }

        private async Task<List<TableDto>> CalculateRank(List<TableDto> tables)
        {
            foreach (var table in tables)
            {
                var biggestMatchday = await _db.Matches.Where(x =>
                    x.LeagueTitle == table.LeagueTitle &&
                    x.Group == table.Group)
                    .OrderByDescending(x => x.Matchday)
                    .FirstAsync();

                table.Matchday = biggestMatchday.Matchday;

                table.Standing = table.Standing
                    .OrderByDescending(y => y.Points)
                    .ThenByDescending(y => y.Goals)
                    .ThenByDescending(y => y.GoalDifference).ToList();

                for (int i = 0; i < table.Standing.Count; i++)
                {
                    table.Standing[i].Rank = i + 1;
                }
            }

            return tables;
        }



        private PtsAndGoalsDto GetPtsAndGoals(MatchDto match)
        {
            var homeTeamPts = MatchHelper.GetNumberOfHomeTeamPoints(match.Score);
            return new PtsAndGoalsDto
            {
                HomeTeamPts = homeTeamPts,
                AwayTeamPts = homeTeamPts == 3 ? 0 : homeTeamPts == 1 ? 1 : 3,
                HomeTeamGoals = MatchHelper.GetNumberOfGoals(match.Score, homeTeam: true),
                AwayTeamGoals = MatchHelper.GetNumberOfGoals(match.Score, homeTeam: false)
            };
        }

        private void InsertNewTable(MatchDto match, PtsAndGoalsDto ptsAndGoals)
        {
            var table = new Table
            {
                LeagueTitle = match.LeagueTitle,
                Matchday = match.Matchday,
                Group = match.Group,
                Standing = new List<Standing>
                    {
                        new Standing(match.HomeTeam,
                            ptsAndGoals.HomeTeamPts,
                            ptsAndGoals.AwayTeamPts,
                            ptsAndGoals.HomeTeamGoals,
                            ptsAndGoals.AwayTeamGoals, 0, true),
                        new Standing(match.AwayTeam,
                            ptsAndGoals.HomeTeamPts,
                            ptsAndGoals.AwayTeamPts,
                            ptsAndGoals.HomeTeamGoals,
                            ptsAndGoals.AwayTeamGoals, 0, false)
                    }
            };

            _db.Tables.Add(table);
        }
    }
}