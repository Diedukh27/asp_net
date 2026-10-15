using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebGenQRCode.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)] // щоб не з'являвся в Swagger
[Route("error")]
public class ErrorController : Controller
{
    [HttpGet("404")]
    public IActionResult NotFoundPage()
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        return View("NotFound");
    }

    [HttpGet("400")]
    public IActionResult BadRequestPage()
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        return View("BadRequest");
    }
}
