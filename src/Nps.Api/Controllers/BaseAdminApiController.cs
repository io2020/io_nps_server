using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nps.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "nps")]
    [ApiController]
    [Authorize]
    [Route("api/nps/[Controller]")]
    public class BaseAdminApiController : ControllerBase
    {

    }
}