using JewelleryStore.Application.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryStore.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class TokenController : BaseController
    {
        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ValidateUserQuery message)
        {
            var (isValidUser, userRno) = await Mediator.Send(message);

            if (isValidUser)
            {
                return new ObjectResult(GenerateToken(message.Id, userRno));
            }
            else
            {
                return BadRequest("Invalid User id or password");
            }
        }

        private dynamic GenerateToken(string username, int userRno)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userRno.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JewelleryStoreSecret")),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                UserRno = userRno
            };
        }
    }
}
