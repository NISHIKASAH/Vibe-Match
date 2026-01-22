using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Controllers;
using API.DTO;
using API.Entitites;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(AppDbContext context)
    {
        if(await context.Users.AnyAsync()) return;
        var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData); // member as no email , so use dto 

        if(members == null)
        {
            Console.WriteLine("No memeber in seed data");
            return;
        }

        
        foreach(var member in members)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Id = member.Id,
                Email = member.Email ,
                DisplayName = member.DisplayName ,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DisplayName = member.DisplayName , 
                    Description = member.Description , 
                    DateOfBirth= member.DateOfBirth, 
                    ImageUrl = member.ImageUrl,
                    Gender = member.Gender ,
                    City = member.City,
                    Country = member.Country , 
                    LastActive = member.LastActive , 
                    Created = member.Created
                }
            };

            user.Member.Photos.Add(
                new Photo
                {
                    Url = member.ImageUrl!,
                    MemberId = member.Id ,



                }
            );
            context.Users.Add(user);
        }
        await context.SaveChangesAsync();
    }

}
