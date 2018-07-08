using HTEC.ChampionsLeague.Utils.Constants;
using HTEC.ChampionsLeague.Utils.Validation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HTEC.ChampionsLeague.Controllers
{
    /// <summary>
    /// Base controller class is inherited by other controllers
    /// </summary>
    [Produces(ProjectConstants.ApplicationJson)]
    [ValidateModel]
    [SwaggerResponse(404, typeof(NotFoundResult))]
    [SwaggerResponse(400, typeof(BadRequestResult))]
    public class BaseController : Controller
    {
    }
}