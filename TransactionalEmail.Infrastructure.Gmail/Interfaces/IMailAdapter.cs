using System.Collections.Generic;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Infrastructure.Gmail.Interfaces
{
    public interface IMailAdapter
    {
        List<long> GetUnreadEmails(IMailboxSettings mailboxSettings);
        Email GetMessageById(IMailboxSettings mailboxSettings, long emailId);
        bool SendEmail(IMailboxSettings mailboxSettings, Email email);
        bool ApplyLabelToMessage(IMailboxSettings mailboxSettings, long emailId, string label);
        bool RemoveLabelFromMessage(IMailboxSettings mailboxSettings, long emailId, string label);
        bool MarkMessageAsRead(IMailboxSettings mailboxSettings, long emailId);
    }
}
