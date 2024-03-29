﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API_RKonnect.Dto;
using API_RKonnect.Models;
using API_RKonnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

public class AuthenticationService : IAuthenticationService
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AuthenticationService(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ActionResult<User>> Register(AuthDto request, DataContext context)
    {
        if (request.Email == null || request.Password == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // Check si l'email est déjà utilisé
        if (await context.Utilisateur.AnyAsync(u => u.Email == request.Email))
        {
            return new BadRequestObjectResult("Email already used.");
        }

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        context.Utilisateur.Add(user);
        await context.SaveChangesAsync();

        return new ActionResult<User>(user);
    }

    public async Task<ActionResult<string>> Login(AuthDto request, DataContext context)
    {
        if (request.Email == null || request.Password == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var user = await context.Utilisateur.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return new BadRequestObjectResult("User not found.");
        }

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return new BadRequestObjectResult("Wrong password.");
        }

        string token = CreateToken(user);

        return new OkObjectResult(token);
    }


    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
