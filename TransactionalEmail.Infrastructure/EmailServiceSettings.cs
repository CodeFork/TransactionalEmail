using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Infrastructure
{
    public class EmailServiceSettings : IEmailServiceSettings
    {
        public bool SendEnabled { get; set; }
    }
}
