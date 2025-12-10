
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entitites;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config ) : ITokenService
{
    
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new
        Exception("cannot get token key");

        if(tokenKey.Length < 64)
        {
            throw  new Exception(" token key must be of length >= 64");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var claims = new  List<Claim> {
            new (ClaimTypes.Email , user.Email) , 
            new (ClaimTypes.NameIdentifier , user.Id)
        };

        var creds = new SigningCredentials(key , SecurityAlgorithms.HmacSha512Signature);

        var tokendescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokendescription);

        return tokenHandler.WriteToken(token);


    }

    
}
