using JewelleryStore.Application.User;
using JewelleryStore.Model.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JewelleryStore.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class TokenController : BaseController
    {
        [Route("token")]
        [HttpPost]
        public async Task<TokenMessage> Create([FromBody] ValidateUserQuery message)
        {
            return await Mediator.Send(message);
        }
    }
}
