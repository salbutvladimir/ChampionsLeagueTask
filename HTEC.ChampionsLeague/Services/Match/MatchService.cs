using AutoMapper;
using HTEC.ChampionsLeague.Context;
using HTEC.ChampionsLeague.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTEC.ChampionsLeague.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationContext _db;
        private readonly ITableService _tableService;

        public MatchService(ApplicationContext db, ITableService tableService)
        {
            _db = db;
            _tableService = tableService;
        }

        public List<MatchDto> GetMatches(MatchFilterDto filter)
        {
            var matches = GetMatcesIQueryable(filter);

            return Mapper.Map<List<MatchDto>>(matches);
        }

        public async Task<List<TableDto>> AddOrUpdateMatches(List<MatchDto> matches)
        {
            foreach (var match in matches)
            {
                if (_db.Matches.Any(x =>
                    x.LeagueTitle == match.LeagueTitle &&
                    x.HomeTeam == match.HomeTeam &&
                    x.AwayTeam == match.AwayTeam))
                {
                    await RemoveMatch(match);
                }
                _db.Matches.Add(Mapper.Map<Match>(match));
                await _tableService.AddOrUpdateTable(match);
                await _db.SaveChangesAsync();
            }
            return await _tableService.GetTables();
        }

        private async Task RemoveMatch(MatchDto match)
        {
            var oldMatch = _db.Matches.Single(x =>
                x.LeagueTitle == match.LeagueTitle &&
                x.HomeTeam == match.HomeTeam &&
                x.AwayTeam == match.AwayTeam);

            _db.Matches.Remove(oldMatch);
            _tableService.RemoveOrUpdateTable(Mapper.Map<MatchDto>(oldMatch));

            await _db.SaveChangesAsync();
        }

        #region private

        private IQueryable<Match> GetMatcesIQueryable(MatchFilterDto filter)
        {
            IQueryable<Match> matches = _db.Matches;

            if (filter.DateFrom.HasValue)
            {
                matches = matches.Where(x => x.KickoffAt > filter.DateFrom);
            }
            if (filter.DateTo.HasValue)
            {
                matches = matches.Where(x => x.KickoffAt < filter.DateTo);
            }
            if (filter.Group?.Length > 0)
            {
                matches = matches.Where(x => filter.Group.Select(s => s.ToLowerInvariant()).Contains(x.Group.ToLowerInvariant()));
            }
            if (filter.Team?.Length > 0)
            {
                matches = matches.Where(x => filter.Team
                .Select(s => s.ToLowerInvariant()).Contains(x.HomeTeam.ToLowerInvariant()) ||
                filter.Team
                .Select(s => s.ToLowerInvariant()).Contains(x.AwayTeam.ToLowerInvariant())
                );
            }

            return matches;
        }

        #endregion private
    }
}