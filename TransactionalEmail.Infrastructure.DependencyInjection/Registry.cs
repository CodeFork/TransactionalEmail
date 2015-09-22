using System.Configuration;
using SimpleInjector;
using SimpleInjector.Packaging;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Rules;
using TransactionalEmail.Core.Services;
using TransactionalEmail.Infrastructure.Data;
using TransactionalEmail.Infrastructure.Gmail;
using TransactionalEmail.Infrastructure.Gmail.Interfaces;

namespace TransactionalEmail.Infrastructure.DependencyInjection
{
    public class Registry : IPackage
    {
        private const string ConnectionName = "EmailsContext";

        public void RegisterServices(Container container)
        {
            //Reference Generator
            container.Register<IReferenceGenerator, CryptographicReferenceGenerator>();

            //Config
            container.RegisterWebApiRequest<IDbSettings>(() => new DbSettings(ConnectionName));

            //DbContext
            container.RegisterWebApiRequest<IEmailContext>(() => new EmailContext(container.GetInstance<IDbSettings>()));
            
            //Services
            container.Register<IEmailService, EmailService>();
            container.Register<IForwardService, ForwardService>();

            //Data
            container.Register<IEmailRepository, EmailRepository>();

            //Validator
            container.Register<IEmailAddressValidator, EmailAddressValidator>();

            //Rules
            container.Register<IForwardingRuleFactory, ForwardingRuleFactory>();

            //Gmail
            container.Register<IEmailProvider, GmailProvider>();
            container.Register<IMailAdapter, LimiLabsAdapter> ();
            container.Register<IOAuth2Authenticator, OAuth2Authenticator>();

            //Config
            container.Register<IMailboxConfiguration>(() => (MailboxConfigurationSettings)(dynamic)ConfigurationManager.GetSection("mailboxConfigurations"));
            container.Register<IMailboxLabels>(() => (MailboxLabelSettings)(dynamic)ConfigurationManager.GetSection("mailboxLabelSettings"));
            container.Register<IGmailSettings>(() => (GmailSettings)(dynamic)ConfigurationManager.GetSection("gmailSettings"));
            container.Register<IEmailServiceSettings>(() => (EmailServiceSettings)(dynamic)ConfigurationManager.GetSection("emailServiceSettings"));

            container.Verify();
        }
    }
}
