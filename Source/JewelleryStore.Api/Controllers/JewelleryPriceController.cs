using JewelleryStore.Application.Price;
using JewelleryStore.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JewelleryStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JewelleryPriceController : BaseController
    {
        [HttpPost("calculate", Name = RouteConstants.CalculatePrice)]
        public async Task<double> Calculate([FromBody] CalculateJewelleryPriceQuery message)
        {
            return await Mediator.Send(message);
        }
    }
}
