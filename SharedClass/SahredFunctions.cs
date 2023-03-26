using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace SharedClass
{
    public class SahredFunctions
    {
        public SahredFunctions(IConfiguration configuration)
        {

        }
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier,user.firstName)
                };
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
