using System;
using System.Collections.Generic;
using System.Linq;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using TransactionalEmail.Core.Objects;
using Attachment = TransactionalEmail.Core.Objects.Attachment;
using Status = TransactionalEmail.Core.Objects.Status;

namespace TransactionalEmail.Infrastructure.Gmail.Mapping
{
    public static class EmailMapper
    {
        public static Email MapEmailToCoreEmail(string accountName, long emailId, IMail email)
        {
            var message = new Email
            {
                AccountName = accountName,
                EmailUid = emailId,
                EmailAddresses = ProcessEmailAddresses(email),
                Subject = email.Subject,
                HtmlBody = email.GetBodyAsHtml(),
                PlainTextBody = email.GetBodyAsText(),
                Date = email.Date?.ToUniversalTime() ?? DateTime.UtcNow,
                Direction = Direction.Inbound,
                Status = Status.Unknown,
            };

            message.Attachments.AddRange(ExtractAttachments(email.NonVisuals));
            message.Attachments.AddRange(ExtractAttachments(email.Visuals));

            return message;
        }

        public static IMail MapToIMail(Email message)
        {
            var builder = new MailBuilder();

            ProcessEmailAddresses(builder, message.EmailAddresses);

            builder.Subject = message.Subject;
            builder.Text = message.PlainTextBody;
            builder.PriorityHigh();

            if (!string.IsNullOrEmpty(message.HtmlBody))
                builder.Html = message.HtmlBody;

            foreach (var ea in message.Attachments)
            {
                var attachment = builder.AddAttachment(ea.ByteArray);
                attachment.FileName = ea.AttachmentName;
                attachment.ContentType = ContentType.Parse(ea.MimeType);
            }

            return builder.Create();
        }

        private static IEnumerable<Attachment> ExtractAttachments(IMimeDataReadOnlyCollection attachments)
        {
            return attachments.Select(attachment => MapToEmailAttachment(attachment.Data, attachment.ContentType.ToString(), attachment.SafeFileName)).ToList();
        }

        private static void ProcessEmailAddresses(MailBuilder builder, IEnumerable<EmailAddress> emailAddresses)
        {
            foreach (var emailAddress in emailAddresses)
            {
                switch (emailAddress.Type)
                {
                    case EmailAddressType.To:
                    {
                        builder.To.Add(new MailBox(emailAddress.Email, emailAddress.Name));
                        break;
                    }
                    case EmailAddressType.From:
                    {
                        builder.From.Add(new MailBox(emailAddress.Email, emailAddress.Name));
                        break;
                    }
                    case EmailAddressType.CarbonCopy:
                    {
                        builder.Cc.Add(new MailBox(emailAddress.Email, emailAddress.Name));
                        break;
                    }
                    case EmailAddressType.BlindCarbonCopy:
                    {
                        builder.Bcc.Add(new MailBox(emailAddress.Email, emailAddress.Name));
                        break;
                    }
                }
            }
        }

        private static List<EmailAddress> ProcessEmailAddresses(IMail email)
        {
            var result = new List<EmailAddress>
            {
                new EmailAddress
                {
                    Name = email.Sender.Name, Email = email.Sender.Address, Type = EmailAddressType.From,
                }
            };

            result.AddRange(ExtractEmailAddresses(email.To, EmailAddressType.To));
            result.AddRange(ExtractEmailAddresses(email.Cc, EmailAddressType.BlindCarbonCopy));
            result.AddRange(ExtractEmailAddresses(email.Bcc, EmailAddressType.BlindCarbonCopy));

            return result;
        }

        private static IEnumerable<EmailAddress> ExtractEmailAddresses(IEnumerable<MailAddress> mailAddresses, EmailAddressType type)
        {
            var result = new List<EmailAddress>();

            foreach (var recipient in mailAddresses)
            {
                result.AddRange(recipient.GetMailboxes().Select(mailbox => new EmailAddress
                {
                    Name = mailbox.Name, Email = mailbox.Address, Type = type,
                }));
            }

            return result;
        }

        private static Attachment MapToEmailAttachment(byte[] data, string contentType, string fileName)
        {
            return new Attachment
            {
                ByteArray = data, MimeType = contentType, AttachmentName = fileName,
            };
        }
    }
}
