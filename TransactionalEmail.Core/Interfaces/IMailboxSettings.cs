namespace TransactionalEmail.Core.Interfaces
{
    public interface IMailboxSettings
    {
        string AccountName { get; set; }
        string MailboxAddress { get; set; }
        string ServerAddress { get; set; }
        int ServerPort { get; set; }
        bool Outbound { get; set; }
        bool ApplyForwardingRules { get; set; }
    }
}
