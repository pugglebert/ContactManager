using ContactManager.Models;
using ContactManager.Data;
using ContactManager.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactManager.Services;

public class AccountService
{
    public IConfiguration _configuration;
    private readonly ContactContext _context;

    public AccountService(IConfiguration configuration, ContactContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public Account? GetByEmail(string email)
    {
        return _context.Accounts
            .AsNoTracking()
            .SingleOrDefault(a => a.Email == email);
    }

    public Account? GetById(long id)
    {
        return _context.Accounts
            .AsNoTracking()
            .SingleOrDefault(a => a.Id == id);
    }


    public Account? Create(AuthRequest auth)
    {
        if (auth.Email == null || auth.Password == null || GetByEmail(auth.Email) != null) return null;

        var newAccount = new Account(auth.Email, BCrypt.Net.BCrypt.HashPassword(auth.Password, 12));

        _context.Accounts.Add(newAccount);
        _context.SaveChanges();
        
        return newAccount;
    }

    public AuthResponse? Authenticate(AuthRequest auth)
    {
        var account = GetByEmail(auth.Email);

        if (account == null || !BCrypt.Net.BCrypt.Verify(auth.Password, account.Password)) return null;
        
        var claims = new [] {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("id", account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, account.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: signIn);

        return new AuthResponse(account.Id, account.Email, new JwtSecurityTokenHandler().WriteToken(token));
    }

    public void DeleteById(long id)
    {
        var accountToDelete = _context.Accounts.Find(id);
        if (accountToDelete is not null)
        {
            _context.Accounts.Remove(accountToDelete);
            _context.SaveChanges();
        }
    }
}