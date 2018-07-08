using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTEC.ChampionsLeague.Dto;
using HTEC.ChampionsLeague.Services;
using HTEC.ChampionsLeague.Utils.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HTEC.ChampionsLeague.Controllers
{
    [Route(ProjectConstants.MatchControllerPath)]
    public class MatchController : BaseController
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        /// <summary>
        /// Get all matches by passed filter
        /// </summary>
        [SwaggerResponse(200, typeof(List<MatchDto>), ProjectConstants.Matches)]
        [HttpGet]
        public IActionResult GetMatches(MatchFilterDto filter)
        {
            var result = _matchService.GetMatches(filter);
            return result != null ? (IActionResult)Ok(result) : BadRequest();
        }

        /// <summary>
        /// Insert and/or update result of matches
        /// </summary>
        [SwaggerResponse(200, typeof(List<TableDto>), ProjectConstants.CurrentLeagueTables)]
        [SwaggerResponse(422, typeof(ValidationResultModel), ProjectConstants.ValidationFailed)]
        [HttpPost]
        public async Task<IActionResult> InsertMatches([FromBody]List<MatchDto> matches)
        {
            if (matches.Any())
            {
                var result = await _matchService.AddOrUpdateMatches(matches);

                return result != null ? (IActionResult)Ok(result) : BadRequest();
            }
            throw new Exception(ProjectConstants.InvalidJSON);
        }
    }
}