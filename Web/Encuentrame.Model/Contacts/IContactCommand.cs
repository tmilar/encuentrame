using System.Collections.Generic;
using Encuentrame.Model.Accounts;

namespace Encuentrame.Model.Contacts
{
    public interface IContactCommand
    {
        IList<Contact> List(int userId);
      

        void RequestContact(ContactCommand.RequestParameters parameters);
        void RejectContact(ContactCommand.RejectParameters parameters);
        void AcceptContact(ContactCommand.AcceptParameters parameters);
        void DeleteContact(ContactCommand.DeleteParameters parameters);

    }
}