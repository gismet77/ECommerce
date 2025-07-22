using ECommerce.Application.Abstractions.Token;
using ECommerce.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;
public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int second, AppUser user)
        {
            Application.DTOs.Token token = new();

            //SecurityKey-in simmetriyini aliriq
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Sifrelenmish kimliyi duzeldirik
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            //Yaradilacaq tokenin ayarlarin veririk
            token.Expiration = DateTime.UtcNow.AddSeconds(second);// tokenin omrunu gosterir
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow, // Token yaradilannan ne vaxt sonra vaxti hesablansin onu bildirir
                signingCredentials: signingCredentials,
                claims: new List<Claim> { new (ClaimTypes.Name, user.UserName) }
                );


            //Token duzelden sinifden bir numune alaq
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            token.RefreshToken = CreateRefreshToken();
            return token;   

             

        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }

    }
}
