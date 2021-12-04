using Ardalis.GuardClauses;
using AspnetRunBasics.Data;
using AspnetRunBasics.Entities;
using System.Threading.Tasks;

namespace AspnetRunBasics.Repositories;

public class ContactRepository : IContactRepository
{
    protected readonly AspnetRunContext _dbContext;

    public ContactRepository(AspnetRunContext dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    public async Task<Contact> SendMessage(Contact contact)
    {
        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> Subscribe(string address)
    {
        // implement your business logic
        Contact newContact = new()
        {
            Email = address,
            Message = address,
            Name = address
        };

        _dbContext.Contacts.Add(newContact);
        await _dbContext.SaveChangesAsync();

        return newContact;
    }
}
