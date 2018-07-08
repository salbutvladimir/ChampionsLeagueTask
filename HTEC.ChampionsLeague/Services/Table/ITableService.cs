using HTEC.ChampionsLeague.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTEC.ChampionsLeague.Services
{
    public interface ITableService
    {
        Task<List<TableDto>> GetTables(TableFilterDto filter = null);

        Task AddOrUpdateTable(MatchDto match);

        void RemoveOrUpdateTable(MatchDto oldMatch);
    }
}
