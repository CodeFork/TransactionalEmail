using System.Collections.Generic;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Interfaces
{
    public interface IEmailProvider
    {
        List<Email> GetEmails(IMailboxSettings mailboxSettings, int numberOfEmailsToRetrieve);
        bool SendEmail(IMailboxSettings mailboxSettings, Email email);
        bool UpdateEmailRetrievalResult(IMailboxSettings mailboxSettings, long emailId, bool retrievedSuccessfully);
    }
}
