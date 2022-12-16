

using Etesian.WebApi.Domain.Interfaces.Data;
using Etesian.WebApi.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etesian.WebApi.Services.Data
{
    public class ContactService : IContactService
    {
        private readonly SiroccoContext _siroccoContext;
        public ContactService(SiroccoContext siroccoContext)
        {
            _siroccoContext = siroccoContext;
        }

        public async Task<Contact> GetContact(long id)
        {
            Contact contact = await Task.Run(() =>
                _siroccoContext.Set<Contact>()
                .Where(x => x.Id == id).FirstOrDefault()
            );
            return contact;
        }

        public async Task<List<Contact>> GetContacts()
        {
            List<Contact> contacts = await Task.Run(() =>
                _siroccoContext.Set<Contact>().ToList()
            );
            return contacts;
        }

        public async Task<long> Insert(Contact contact)
        {
            _siroccoContext.Set<Contact>().Add(contact);
            await Task.Run(() => 
                _siroccoContext.SaveChanges()
            );
            return contact.Id;
        }

        public async Task Update(Contact contact)
        {
            _siroccoContext.Set<Contact>().Update(contact);
            await Task.Run(() => 
                _siroccoContext.SaveChanges()
            );
            return;
        }

        public async Task Delete(Contact contact)
        {
            _siroccoContext.Set<Contact>().Remove(contact);
            await Task.Run(() => 
                _siroccoContext.SaveChanges()
            );
            return;
        }

        public async Task<List<Contact>> GetContactsForCustomer(long customerId)
        {
            List<Contact> contacts = await Task.Run(() => 
                _siroccoContext.Set<Contact>()
                .Where(x => x.CustomerId == customerId).ToList()
            );
            return contacts;
        }
    }
}
