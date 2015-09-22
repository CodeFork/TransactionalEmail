using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Infrastructure.Gmail.Interfaces;

namespace TransactionalEmail.Infrastructure.Gmail
{
    public class GmailProvider : IEmailProvider
    {
        private readonly object _key = new object();
        private readonly IMailAdapter _mailAdaptor;
        private readonly IMailboxLabels _mailboxLabels;

        public GmailProvider(IMailAdapter mailAdaptor, IMailboxLabels mailboxLabels)
        {
            Check.If(mailAdaptor).IsNotNull();
            Check.If(mailboxLabels).IsNotNull();

            _mailAdaptor = mailAdaptor;
            _mailboxLabels = mailboxLabels;
        }

        public List<Email> GetEmails(IMailboxSettings mailboxSettings, int numberOfEmailsToRetrieve)
        {
            List<Email> emails;

            lock (_key)
            {
                var ids = _mailAdaptor.GetUnreadEmails(mailboxSettings);

                if (ids.Any() && numberOfEmailsToRetrieve > 0)
                    ids = ids.Take(numberOfEmailsToRetrieve).ToList();

                emails = DownloadEmails(mailboxSettings, ids);
            }

            return emails;
        }

        public bool SendEmail(IMailboxSettings mailboxSettings, Email email)
        {
            bool success;

            lock (_key)
            {
                success = _mailAdaptor.SendEmail(mailboxSettings, email);
            }

            return success;
        }

        public bool UpdateEmailRetrievalResult(IMailboxSettings mailboxSettings, long emailId, bool retrievedSuccessfully)
        {
            var success = true;

            lock (_key)
            {
                if (retrievedSuccessfully)
                    success = _mailAdaptor.MarkMessageAsRead(mailboxSettings, emailId);

                if (success)
                    success = _mailAdaptor.RemoveLabelFromMessage(mailboxSettings, emailId,
                        _mailboxLabels.InboundMailBoxProcessingLabel);
            }

            return success;
        }

        private List<Email> DownloadEmails(IMailboxSettings mailboxSettings, IEnumerable<long> ids)
        {
            var emails = new List<Email>();
            var retrieveTasks = new List<Task>();

            foreach (var id in ids)
            {
                var retrieveTask = Task<Email>.Factory.StartNew(() => GetMessage(mailboxSettings, id));

                retrieveTasks.Add(retrieveTask);

                if (retrieveTask.Result != null)
                    emails.Add(retrieveTask.Result);
            }

            Task.WaitAll(retrieveTasks.ToArray());

            return emails;
        }

        private Email GetMessage(IMailboxSettings mailboxSettings, long emailId)
        {
            var email = _mailAdaptor.GetMessageById(mailboxSettings, emailId);
            _mailAdaptor.ApplyLabelToMessage(mailboxSettings, emailId, _mailboxLabels.InboundMailBoxProcessingLabel);
            return email;
        }
    }
}
