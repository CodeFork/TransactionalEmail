namespace TransactionalEmail.Core.Interfaces
{
    public interface IMailboxLabels
    {
        string InboundMailBoxProcessingLabel { get; set; }
        string InboundMailBoxErrorLabel { get; set; }
    }
}
