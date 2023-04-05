using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASP.NETCore_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ASP.NETCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration configuration;

        public UserInfo currentUser;
        public TokenController(IConfiguration config)
        {
            configuration = config;
            // mock an user
            currentUser = new UserInfo()
            {
                
                Email = "minh.nguyenvan@ncc.asia",
                LastName = "Nguyễn Văn",
                FirstName = "Minh",
                Password = "4nhnho3m",
                UserId = 4,
                UserName = "minh.nguyen",
                CreatedDate = DateTime.Now
            };
        }

        [HttpGet]
        public IActionResult get(UserInfo userData)
        {

            if (userData != null && userData.UserName != null && userData.Password != null)
            {
                var user = GetUser(userData.UserName, userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

                    new Claim("Password", user.Password),
                    new Claim("UserName", user.UserName)
                    
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private UserInfo GetUser(string UserName, string password)
        {
            if (currentUser.UserName != UserName || currentUser.Password != password)
                return null;
            return currentUser;
        }
    }
}

