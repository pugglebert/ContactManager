using Microsoft.AspNetCore.Mvc;
using ContactManager.Services;
using ContactManager.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ContactManager.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private AccountService _service;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public IActionResult Register(AuthRequest auth)
    {
        if (auth.Email is null) return BadRequest("Email is required.");
        if (auth.Password is null) return BadRequest("Password is required.");

        var account = _service.Create(auth);
        if (account is null) return BadRequest($"An accont with the email {auth.Email} already exists.");

        var authResponse = _service.Authenticate(auth);
        return Ok(authResponse);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public IActionResult Login(AuthRequest auth)
    {
        if (auth.Email is null) return BadRequest("Email is required.");
        if (auth.Password is null) return BadRequest("Password is required.");

        var authResponse = _service.Authenticate(auth);
        if (authResponse is null) return BadRequest("Invalid email or password.");

        return Ok(authResponse);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult Delete(long id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity is null) return Unauthorized();
        if (!IsOwnerPredicate(identity, id.ToString())) return Forbid();

        var account = _service.GetById(id);
        if (account is null) return NotFound();
        _service.DeleteById(id);
        return NoContent();
    }

    private Boolean IsOwnerPredicate(ClaimsIdentity identity, string id)
    {
        var idClaim = identity.Claims.FirstOrDefault(claim => claim.Type == "id");
        if (idClaim is null) return false;
        return idClaim.Value == id;
    }
}