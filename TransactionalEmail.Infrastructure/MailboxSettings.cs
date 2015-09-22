using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Infrastructure
{
    public class MailboxSettings : IMailboxSettings
    {
        public string AccountName { get; set; }
        public string MailboxAddress { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public bool Outbound { get; set; }
        public bool ApplyForwardingRules { get; set; }
    }
}
