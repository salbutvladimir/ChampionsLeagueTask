using HTEC.ChampionsLeague.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTEC.ChampionsLeague.Services
{
    public interface IMatchService
    {
        List<MatchDto> GetMatches(MatchFilterDto filter);

        Task<List<TableDto>> AddOrUpdateMatches(List<MatchDto> matches);
    }
}
