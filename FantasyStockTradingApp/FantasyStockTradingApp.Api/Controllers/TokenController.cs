using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FantasyStockTradingApp.Models;
using FantasyStockTradingApp.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FantasyStockTradingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AuthOptions _authOptions;

        public TokenController(IUserService userService, IOptions<AuthOptions> authOptionAccessor)
        {
            _authOptions = authOptionAccessor.Value;
            _userService = userService;
        }

        [HttpGet("get_token")]
        public IActionResult GetToken(string email, string password)
        {
            if (_userService.isValidUser(email, password))
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
