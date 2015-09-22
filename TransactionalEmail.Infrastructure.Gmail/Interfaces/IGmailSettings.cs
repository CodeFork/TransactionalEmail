namespace TransactionalEmail.Infrastructure.Gmail.Interfaces
{
    public interface IGmailSettings
    {
        string ServiceAccountEmailAddress { get; set; }
        string ServiceAccountCertPath { get; set; }
        string ServiceAccountCertPassword { get; set; }
    }
}
