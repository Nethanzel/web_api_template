using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dataCoreService.Controllers
{
    [Route("api")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpGet("people/all")]
        public ActionResult<IEnumerable<Person>> GetAll([FromHeader(Name = "token")][Required] string requiredHeader)
        {
            return new[]
            {
                new Person { Name = "Ana" },
                new Person { Name = "Felipe" },
                new Person { Name = "Emillia" }
            };
        }
    }
}


public class Person
{
    public string Name { get; set; }
}