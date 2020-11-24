using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FantasyStockTradingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserLoginService _userLoginService;
        private readonly AuthOptions _authOptions;

        public TokenController(IUserLoginService userLoginService, IOptions<AuthOptions> authOptionAccessor)
        {
            _authOptions = authOptionAccessor.Value;
            _userLoginService = userLoginService;
        }

        [HttpGet("get_token")]
        public IActionResult GetToken(string email, string password)
        {
            if (_userLoginService.isValidUser(email, password))
            {
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(_authOptions.ExpiresInHours),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecureKey)),
                    SecurityAlgorithms.HmacSha256Signature)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiraction = token.ValidTo
                });
            }
            return Unauthorized();
        }

    }
}
