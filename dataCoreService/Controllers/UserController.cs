using dataCoreService.Data;
using dataCoreService.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace dataCoreService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryInterface<User> repository = null;
        public UserController(IRepositoryInterface<User> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Index([FromHeader(Name = "token")][Required] string requiredHeader)
        {
            var model = repository.GetAll();
            return Ok(model);
        }
    }
}
