using HTEC.ChampionsLeague.Dto;
using HTEC.ChampionsLeague.Services;
using HTEC.ChampionsLeague.Utils.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTEC.ChampionsLeague.Controllers
{
    [Route(ProjectConstants.TableControllerPath)]
    public class TableController : BaseController
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        /// <summary>
        /// Get league table and standings by passed filter
        /// </summary>
        [SwaggerResponse(200, typeof(List<TableDto>), ProjectConstants.LeagueTables)]
        [HttpGet]
        public async Task<IActionResult> GetTables(TableFilterDto filter)
        {
            var result = await _tableService.GetTables(filter);

            return result != null ? (IActionResult)Ok(result) : BadRequest();
        }
    }
}