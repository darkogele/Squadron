using SquadronApi.Entities;
using System.Security.Cryptography;
using System.Text;

namespace SquadronApi.Data;
public static class Seed
{
    public static async Task SeedData(DataContext context, IConfiguration config)
    {
        if (!context.Users.Any())
        {
            CreatePasswordHash(config["SeedUser:password"], out var passwordHash, out var passwordSalt);

            var user = new User
            {
                DisplayName = "Admin User",
                Email = config["SeedUser:email"],
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();
        }
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}