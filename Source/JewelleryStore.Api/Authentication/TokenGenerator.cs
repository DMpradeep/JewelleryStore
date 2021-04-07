using JewelleryStore.Application.Common;
using JewelleryStore.Model.Common;
using JewelleryStore.Model.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JewelleryStore.Api.Authentication
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IOptions<AuthenticationSetting> _config;

        public TokenGenerator(IOptions<AuthenticationSetting> config)
            => _config = config;

        public TokenMessage GenerateToken(int userRno, string userId)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.NameIdentifier, userRno.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.SymmetricSecurityKey)),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new TokenMessage()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserRno = userRno
            };
        }
    }
}
