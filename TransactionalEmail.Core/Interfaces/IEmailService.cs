using System.Collections.Generic;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Interfaces
{
    public interface IEmailService
    {
        List<Email> RetrieveMessages(int numberOfEmailsToRetrieve);
        bool Send(Email email);
        bool NotifyRetrievalResult(string emailReference, bool retrieved);
    }
}
