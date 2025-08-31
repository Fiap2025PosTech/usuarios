using Microsoft.AspNetCore.Mvc;

namespace Usuarios.API.Controllers;

[ApiController]
public class BaseController(ILogger<BaseController> logger) : ControllerBase
{
    private readonly ILogger<BaseController> _logger = logger;
}