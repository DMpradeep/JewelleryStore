using JewelleryStore.Api.Authorization;
using JewelleryStore.Application.Price;
using JewelleryStore.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JewelleryStore.Api.Controllers
{
    [Route("api/{userRno}/[controller]")]
    [ApiController]
    [Authorize(Policy = Permissions.User.Manage)]
    public class JewelleryPriceController : BaseController
    {
        [HttpPost("calculate", Name = RouteConstants.CalculatePrice)]
        public async Task<double> Calculate(int userRno, [FromBody] CalculateJewelleryPriceQuery message)
        {
            message.UserRno = userRno;
            return await Mediator.Send(message);
        }
    }
}
