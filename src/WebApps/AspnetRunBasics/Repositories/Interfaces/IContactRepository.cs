using AspnetRunBasics.Entities;

namespace AspnetRunBasics.Repositories
{
    public interface IContactRepository
    {
        Task<Contact> SendMessage(Contact contact);
        Task<Contact> Subscribe(string address);
    }
}
