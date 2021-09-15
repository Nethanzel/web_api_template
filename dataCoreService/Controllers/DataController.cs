using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataCoreService.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpGet("people/all")]
        public ActionResult<IEnumerable<Person>> GetAll()
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