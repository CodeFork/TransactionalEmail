using System.Collections.Generic;
using AutoMapper;
using Conditions.Guards;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Extensions;
using Attachment = TransactionalEmail.Models.Attachment;
using Email = TransactionalEmail.Models.Email;

namespace TransactionalEmail.Factories
{
    public class EmailFactory
    {
        public static Core.Objects.Email CreateCoreEmail(Email emailModel)
        {
            Check.If(emailModel).IsNotNull();

            var result = new Core.Objects.Email
            {
                Subject = emailModel.Subject,
                PlainTextBody = emailModel.PlainTextBody,
                HtmlBody = emailModel.HtmlBody,
                Attachments = Mapper.Map<List<Attachment>, List<Core.Objects.Attachment>> (emailModel.Attachments)
            };

            result.EmailAddresses.AddRange(emailModel.ToAddresses.ToEmailAddressList(EmailAddressType.To));
            result.EmailAddresses.AddRange(emailModel.FromAddress.ToEmailAddressList(EmailAddressType.From));
            result.EmailAddresses.AddRange(emailModel.Bcc.ToEmailAddressList(EmailAddressType.BlindCarbonCopy));
            result.EmailAddresses.AddRange(emailModel.Cc.ToEmailAddressList(EmailAddressType.CarbonCopy));

            return result;
        }

        public static Email CreateEmailModel(Core.Objects.Email email)
        {
            Check.If(email).IsNotNull();

            return new Email
            {
                EmailReference = email.EmailReference,
                ToAddresses = email.ToAddresses.ToAddressString(),
                FromAddress = email.FromAddress.ToAddressString(),
                Cc = email.Ccs.ToAddressString(),
                Bcc = email.Bccs.ToAddressString(),
                Subject = email.Subject,
                PlainTextBody = email.PlainTextBody,
                HtmlBody = email.HtmlBody,
                Attachments = Mapper.Map<List<Core.Objects.Attachment>, List<Attachment>>(email.Attachments)
            };
        }
    }
}