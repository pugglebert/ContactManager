using ContactManager.Models;
using ContactManager.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Services;

public class MemberService
{
    private readonly ContactContext _context;

    public MemberService(ContactContext context)
    {
        _context = context;
    }

    public IEnumerable<Member> GetAll()
    {
        return _context.Members
            .Include(m => m.Contact)
            .AsNoTracking()
            .ToList();
    }

    public Member? GetById(long id)
    {
        return _context.Members
            .Include(m => m.Contact)
            .AsNoTracking()
            .SingleOrDefault(m => m.Id == id);
    }

    public Member Create(Member newMember)
    {
        _context.Members.Add(newMember);
        _context.SaveChanges();
        
        return newMember;
    }

    public void DeleteById(long id)
    {
        var memberToDelete = _context.Members.Find(id);
        if (memberToDelete is not null)
        {
            _context.Members.Remove(memberToDelete);
            _context.SaveChanges();
        }
    }

}