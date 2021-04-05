using JewelleryStore.Application.User;
using JewelleryStore.Model.Common;
using JewelleryStore.Model.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JewelleryStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        [HttpGet("{userRno}", Name = RouteConstants.User)]
        public async Task<UserMessage> Get(int userRno)
        {
            var query = new UserDetailsQuery() { UserRno = userRno };
            return await Mediator.Send(query);
        }

        [HttpPost("validate", Name = RouteConstants.ValidateUser)]
        public async Task<int> Validate([FromBody] ValidateUserQuery message)
        {
            return await Mediator.Send(message);
        }
    }
}
