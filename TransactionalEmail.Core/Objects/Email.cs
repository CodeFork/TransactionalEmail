using System;
using System.Collections.Generic;
using System.Linq;
using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Core.Objects
{
    public class Email
    {
        public int EmailId { get; set; }
        public string EmailReference { get; set; }
        public string AccountName { get; set; }
        public long? EmailUid { get; set; }
        public List<EmailAddress> EmailAddresses { get; set; } 
        public string Subject { get; set; }
        public string PlainTextBody { get; set; }
        public string HtmlBody { get; set; }
        public DateTime? Date { get; set; }
        public List<Attachment> Attachments { get; set; }
        public Direction Direction { get; set; }
        public List<AppliedRule> AppliedRules { get; set; } 
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }

        public Email()
        {
            EmailId = 0;
            EmailReference = string.Empty;
            AccountName = string.Empty;
            EmailUid = null;
            EmailAddresses = new List<EmailAddress>();
            Subject = string.Empty;
            PlainTextBody = string.Empty;
            HtmlBody = string.Empty;
            Attachments = new List<Attachment>();
            Direction = Direction.Unknown;
            AppliedRules = new List<AppliedRule>();
            Status = Status.Unknown;
            DateCreated = DateTime.UtcNow;
        }

        public Email CreateReference(IReferenceGenerator referenceGenerator)
        {
            Check.If(referenceGenerator).IsNotNull();

            EmailReference = referenceGenerator.CreateReference(Constants.ReferenceLength);

            return this;
        }

        public Email SetDirection(Direction direction)
        {
            Direction = direction;

            return this;
        }

        public Email SetAccountName(string accountName)
        {
            AccountName = accountName;

            return this;
        }

        public EmailAddress FromAddress
        {
            get { return EmailAddresses.FirstOrDefault(x => x.Type == EmailAddressType.From); }
        }

        public List<EmailAddress> ToAddresses
        {
            get { return EmailAddresses.Where(x => x.Type == EmailAddressType.To).ToList(); }
        }

        public List<EmailAddress> Ccs
        {
            get { return EmailAddresses.Where(x => x.Type == EmailAddressType.CarbonCopy).ToList(); }
        }

        public List<EmailAddress> Bccs
        {
            get { return EmailAddresses.Where(x => x.Type == EmailAddressType.BlindCarbonCopy).ToList(); }
        }
    }
}
