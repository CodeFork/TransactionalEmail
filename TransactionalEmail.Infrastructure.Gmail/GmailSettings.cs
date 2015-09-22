using TransactionalEmail.Infrastructure.Gmail.Interfaces;

namespace TransactionalEmail.Infrastructure.Gmail
{
    public class GmailSettings : IGmailSettings
    {
        public string ServiceAccountEmailAddress { get; set; }
        public string ServiceAccountCertPath { get; set; }
        public string ServiceAccountCertPassword { get; set; }
    }
}
