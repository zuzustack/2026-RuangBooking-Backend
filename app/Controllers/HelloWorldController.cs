using Microsoft.AspNetCore.Mvc;

namespace RuangBooking.Controllers;
[ApiController]
[Route("[controller]")]
public class HelloWorldController : ControllerBase
{
    [HttpGet(Name = "GetHelloWorld")]
    // Json
    public IActionResult Get()
    {
        return new JsonResult(new {
            message = "Hello World"
        });
    }
}