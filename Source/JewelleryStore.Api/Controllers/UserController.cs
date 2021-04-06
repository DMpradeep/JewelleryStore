using JewelleryStore.Application.User;
using JewelleryStore.Model.Common;
using JewelleryStore.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JewelleryStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        [HttpGet("{userRno}", Name = RouteConstants.User)]
        public async Task<UserMessage> Get(int userRno)
        {
            var query = new UserDetailsQuery() { UserRno = userRno };
            return await Mediator.Send(query);
        }
    }
}
