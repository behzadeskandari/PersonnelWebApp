﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Personnel.Application.Interfaces;
using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Application.Services
{
    public class TokenServices : ITokenService
    {
        private readonly IConfiguration _configuration;

        private readonly SymmetricSecurityKey _key;

        public TokenServices(IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        public string CreateToken(User user)
        {

            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Use 'Sub' for subject claim
                 new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),  // Use 'UniqueName' for username claim
            };
            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var TokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred,

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(TokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public string GetToken(User user)
        {
            return CreateToken(user);
        }
    }
}
