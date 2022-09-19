using Microsoft.EntityFrameworkCore;
using ContactManager.Models;

namespace ContactManager.Data;

public class ContactContext : DbContext
{
    public ContactContext (DbContextOptions<ContactContext> options)
        : base(options)
    {
        
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Member> Members => Set<Member>();
}