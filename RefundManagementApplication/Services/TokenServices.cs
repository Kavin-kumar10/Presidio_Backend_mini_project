﻿using Microsoft.IdentityModel.Tokens;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RefundManagementApplication.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly string _secretKey;
        private readonly SymmetricSecurityKey _key;

        #region Constructor
        public TokenServices(IConfiguration configuration)
        {
            _secretKey = configuration.GetSection("TokenKey").GetSection("JWT").Value.ToString();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }
        #endregion

        #region Generate Token
        /// <summary>
        /// Generate Token from the Member data received from the User
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public string GenerateToken(Member member)
        {
            string token = string.Empty;
            var claims = new List<Claim>(){
                new Claim(ClaimTypes.Name, member.Id.ToString()),
                new Claim(ClaimTypes.Role,member.Role.ToString())
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddDays(2), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(myToken);
            return token;
        }
        #endregion
    }
}
