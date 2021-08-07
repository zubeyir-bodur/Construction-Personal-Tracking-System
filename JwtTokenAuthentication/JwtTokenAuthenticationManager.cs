using Construction_Personal_Tracking_System.Deneme;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Construction_Personal_Tracking_System.JwtTokenAuthentication {
    public class JwtTokenAuthenticationManager : IJwtTokenAuthenticationManager {

        public readonly string securityKey;
        public readonly PersonelTakipDBContext context;
        public readonly IConfiguration Configuration;

        public JwtTokenAuthenticationManager(PersonelTakipDBContext context, IConfiguration configuration) {
            this.Configuration = configuration;
            this.securityKey = Configuration.GetSection("SecretKey").GetSection("Key").Value;
            this.context = context;
        }

        /// <summary>
        /// Authenticate user via username and password
        /// </summary>
        /// <author>Furkan Calik</author>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>String: Bearer Token </returns>
        public string Authenticate(string username, string password) {

            Personnel personnel = context.Personnel.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            PersonnelType role = context.PersonnelTypes.Where(u => u.PersonnelTypeId == personnel.PersonnelTypeId).FirstOrDefault();
            if(personnel == null) {
                return null;
            }
            if (!personnel.Password.Equals(password)) {
                return null;
            }

            //Creating Token regarding security key
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(securityKey);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role.PersonnelTypeName)
                }),
                // Expiration time: 1 hours
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
