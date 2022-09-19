using Microsoft.AspNetCore.Mvc;
using ContactManager.Services;
using ContactManager.Models;

namespace ContactManager.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberController : ControllerBase
{
    private MemberService _service;

    public MemberController(MemberService service)
    {
        _service = service;
    }

    [HttpGet]
    public IEnumerable<Member> GetAll() {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Member> GetById(long id)
    {
        var Member = _service.GetById(id);
        if (Member is null) return NotFound();
        return Member;
    }

    [HttpPost]
    public IActionResult Create(Member newMember)
    {
        var member = _service.Create(newMember);
        return CreatedAtAction(nameof(Create), new { id = member.Id }, member);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var Contact = _service.GetById(id);
        if (Contact is null) return NotFound();
        _service.DeleteById(id);
        return NoContent();
    }
}