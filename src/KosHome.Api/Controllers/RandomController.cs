using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RandomController : ControllerBase
{
    public RandomController()
    {
        
    }
    [HttpGet(Name = "GetRandomInt")]
    public IEnumerable<int> Get()
    {
        return Enumerable.Range(1, 5);
    }
}