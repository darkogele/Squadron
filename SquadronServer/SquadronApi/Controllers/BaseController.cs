using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquadronApi.Core;

namespace SquadronApi.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize]
public class BaseController : ControllerBase
{
    protected ActionResult HandleResult<T>(ServerResponse<T> result)
    {
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
    }
}