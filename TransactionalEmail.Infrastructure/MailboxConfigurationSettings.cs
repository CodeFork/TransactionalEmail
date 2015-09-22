using System.Collections.Generic;
using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Infrastructure
{
    public class MailboxConfigurationSettings : IMailboxConfiguration
    {
        public List<IMailboxSettings> Mailboxes { get; set; }
    }
}
