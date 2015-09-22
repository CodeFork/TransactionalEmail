using System;
using System.Collections.Generic;
using Conditions.Guards;
using Limilabs.Client.IMAP;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Infrastructure.Gmail.Interfaces;
using TransactionalEmail.Infrastructure.Gmail.Mapping;

namespace TransactionalEmail.Infrastructure.Gmail
{
    public class LimiLabsAdapter : IMailAdapter
    {
        private readonly IOAuth2Authenticator _oAuth2Authenticator;
        private readonly IMailboxLabels _mailboxLabels;

        public LimiLabsAdapter(IOAuth2Authenticator oAuth2Authenticator, IMailboxLabels mailboxLabels)
        {
            Check.If(oAuth2Authenticator).IsNotNull();
            Check.If(mailboxLabels).IsNotNull();

            _oAuth2Authenticator = oAuth2Authenticator;
            _mailboxLabels = mailboxLabels;
        }

        public List<long> GetUnreadEmails(IMailboxSettings mailboxSettings)
        {
            List<long> result;

            using (var imap = new Imap())
            {
                try
                {
                    var loggedIn = LoginToInbox(mailboxSettings, imap);

                    if (!loggedIn)
                        return null;

                    var searchCriteria = BuildSearchCriteria();
                    result = imap.Search(Expression.And(searchCriteria));
                }
                catch (Exception ex)
                {
                    result = new List<long>();
                }
            }

            return result;
        }

        private ICriterion[] BuildSearchCriteria()
        {
            return new[]
            {
                Expression.HasFlag(Flag.Unseen),
                Expression.Not(Expression.GmailLabel(_mailboxLabels.InboundMailBoxProcessingLabel)),
                Expression.Not(Expression.GmailLabel(_mailboxLabels.InboundMailBoxErrorLabel))
            };
        }

        public Email GetMessageById(IMailboxSettings mailboxSettings, long emailId)
        {
            Email result = null;

            using (var imap = new Imap())
            {
                try
                {
                    var loggedIn = LoginToInbox(mailboxSettings, imap);

                    if (!loggedIn)
                        return null;

                    var eml = imap.PeekMessageByUID(emailId);
                    var email = new MailBuilder().CreateFromEml(eml);

                    result = EmailMapper.MapEmailToCoreEmail(mailboxSettings.AccountName, emailId, email);
                }
                catch (Exception ex)
                {
                    ApplyLabelToMessage(mailboxSettings, emailId, _mailboxLabels.InboundMailBoxErrorLabel);
                    MarkMessageAsRead(mailboxSettings, emailId);
                }
            }

            return result;
        }

        public bool SendEmail(IMailboxSettings mailboxSettings, Email email)
        {
            using (var client = new Smtp())
            {
                try
                {
                    client.ConnectSSL(mailboxSettings.ServerAddress, mailboxSettings.ServerPort);
                    client.LoginOAUTH2(email.FromAddress.Email, _oAuth2Authenticator.GetOAuth2AccessToken(email.FromAddress.Email));

                    return client.SendMessage(EmailMapper.MapToIMail(email)).Status == SendMessageStatus.Success;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool ApplyLabelToMessage(IMailboxSettings mailboxSettings, long emailId, string label)
        {
            var result = true;

            using (var imap = new Imap())
            {
                try
                {
                    var loggedIn = LoginToInbox(mailboxSettings, imap);

                    if (!loggedIn)
                        return false;

                    imap.GmailLabelMessageByUID(emailId, label);
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }

            return result;
        }
        public bool RemoveLabelFromMessage(IMailboxSettings mailboxSettings, long emailId, string label)
        {
            var result = true;

            using (var imap = new Imap())
            {
                try
                {
                    var loggedIn = LoginToInbox(mailboxSettings, imap);

                    if (!loggedIn)
                        return false;

                    imap.GmailUnlabelMessageByUID(emailId, label);
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }

            return result;
        }

        public bool MarkMessageAsRead(IMailboxSettings mailboxSettings, long emailId)
        {
            var result = true;

            using (var imap = new Imap())
            {
                try
                {
                    var loggedIn = LoginToInbox(mailboxSettings, imap);

                    if (!loggedIn)
                        return false;

                    imap.FlagMessageByUID(emailId, Flag.Seen);
                }
                catch (Exception ex)
                {
                    result = false;
                }

                imap.Close();
            }

            return result;
        }

        private bool LoginToInbox(IMailboxSettings mailboxSettings, Imap imap)
        {
            imap.ConnectSSL(mailboxSettings.ServerAddress, mailboxSettings.ServerPort);

            var accessToken = _oAuth2Authenticator.GetOAuth2AccessToken(mailboxSettings.MailboxAddress);

            if (string.IsNullOrEmpty(accessToken))
                return false;

            imap.LoginOAUTH2(mailboxSettings.AccountName, accessToken);

            imap.SelectInbox();

            return true;
        }
    }
}
