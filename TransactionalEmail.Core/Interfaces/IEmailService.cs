using System.Collections.Generic;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Interfaces
{
    public interface IEmailService
    {
        List<Email> RetrieveMessages(int numberOfEmailsToRetrieve);
        Email GetEmail(string emailReference);
        string Send(Email email);
        bool NotifyRetrievalResult(string emailReference, bool retrieved);
    }
}
