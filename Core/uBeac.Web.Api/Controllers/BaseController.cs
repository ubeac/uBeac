using Microsoft.AspNetCore.Mvc;
using uBeac.Web.Api.Filters;

namespace uBeac.Web.Api.Controllers
{
    [Route("api/[controller]/[action]/")]
    [TypeFilter(typeof(ModelStateValidationFilter))]
    [TypeFilter(typeof(ResultSetResponseCodeFilter))]

    public abstract class BaseController
    {
    }
}
