using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KosHome.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RandomController : ControllerBase
{
    [HttpGet(Name = "GetRandomInt")]
    public IEnumerable<int> Get()
    {
        return Enumerable.Range(1, 5);
    }
}