using System.Collections.Generic;

namespace TransactionalEmail.Core.Interfaces
{
    public interface IMailboxConfiguration
    {
        List<IMailboxSettings> Mailboxes { get; set; } 
    }
}
