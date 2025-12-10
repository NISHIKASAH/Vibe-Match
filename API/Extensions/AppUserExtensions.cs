using System;
using API.DTO;
using API.Entitites;
using API.Interfaces;

namespace API.Extensions;

public static class AppUserExtensions
{

public static UserDto ToDto (this AppUser user, ITokenService tokenService) //cannot do DI (bcoz static class)
    {
          return new UserDto // created this to send token to user 
            {
                Id = user.Id ,
                DisplayName = user.DisplayName ,
                Email = user.Email ,
                Token = tokenService.CreateToken(user)
            };
    }

}
