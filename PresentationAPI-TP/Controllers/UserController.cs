using Contracts.User.Response;
using Microsoft.AspNetCore.Mvc;

namespace PresentationAPI_TP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetUserById([FromRoute]int id)
        {
            
            var fakeUser = new UserResponse
            {
                Id = id,
                Email = "prueba@test.com"
            };

            return Ok(fakeUser);
        }
    }
}
