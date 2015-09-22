using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Infrastructure
{
    public class MailboxLabelSettings : IMailboxLabels
    {
        public string InboundMailBoxProcessingLabel { get; set; }
        public string InboundMailBoxErrorLabel { get; set; }
    }
}
