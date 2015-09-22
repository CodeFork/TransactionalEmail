using System.Collections.Generic;
using System.Linq;
using Conditions;
using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IMailboxConfiguration _mailboxConfiguration;
        private readonly IForwardService _forwardService;
        private readonly IEmailProvider _emailProvider;
        private readonly IEmailRepository _emailRepository;
        private readonly IReferenceGenerator _referenceGenerator;

        public EmailService(IMailboxConfiguration mailboxConfiguration, 
                            IForwardService forwardService,
                            IEmailProvider emailProvider, 
                            IEmailRepository emailRepository, 
                            IReferenceGenerator referenceGenerator)
        {
            Check.If(mailboxConfiguration).IsNotNull();
            Check.If(forwardService).IsNotNull();
            Check.If(emailProvider).IsNotNull();
            Check.If(emailRepository).IsNotNull();
            Check.If(referenceGenerator).IsNotNull();

            _mailboxConfiguration = mailboxConfiguration;
            _forwardService = forwardService;
            _emailProvider = emailProvider;
            _emailRepository = emailRepository;
            _referenceGenerator = referenceGenerator;
        }

        public List<Email> RetrieveMessages(int numberOfEmailsToRetrieve)
        {
            var result = new List<Email>();

            foreach (var mailboxSettings in _mailboxConfiguration.Mailboxes.Where(x => !x.Outbound))
            {
                var emails = _emailProvider.GetEmails(mailboxSettings, numberOfEmailsToRetrieve);

                foreach (var email in emails)
                {
                    _emailRepository.CreateEmail(email);

                    var processedEmail = ApplyProcessingRules(mailboxSettings, email);

                    _emailRepository.UpdateAppliedRules(email.EmailReference, processedEmail.RuleApplied);

                    result.Add(processedEmail.EmailResult);
                }
            }

            return result;
        }

        public bool Send(Email email)
        {
            var config = _mailboxConfiguration.Mailboxes.FirstOrDefault(x => x.Outbound);

            if (config.IsNull())
                return false;

            _emailRepository.CreateEmail(email.CreateReference(_referenceGenerator).SetDirection(Direction.Outbound).SetAccountName(config.AccountName));
            
            var result = _emailProvider.SendEmail(config, email);

            _emailRepository.UpdateStatus(email.EmailReference, result ? Status.Success : Status.Error);

            return result;
        }

        public bool NotifyRetrievalResult(string emailReference, bool retrieved)
        {
            var result = false;

            var email = _emailRepository.GetEmailByReference(emailReference);

            if (email.IsNull())
                return false;

            var mailboxSettings = _mailboxConfiguration.Mailboxes.FirstOrDefault(x => x.AccountName == email.AccountName);

            if (mailboxSettings.IsNotNull())
            {
                result = _emailRepository.UpdateStatus(emailReference, retrieved ? Status.Success : Status.Error);
                result &= _emailProvider.UpdateEmailRetrievalResult(mailboxSettings, email.EmailId, retrieved);
            }

            return result;
        }

        private ForwardResult ApplyProcessingRules(IMailboxSettings mailboxSettings, Email email)
        {
            return mailboxSettings.ApplyForwardingRules
                ? _forwardService.ProcessEmail(email)
                : new ForwardResult {EmailResult = email};
        }
    }
}
