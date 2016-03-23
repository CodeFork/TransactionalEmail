using System;
using System.Collections.Generic;
using System.Linq;
using Conditions;
using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Infrastructure.Data
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IEmailContext _emailContext;

        public EmailRepository(IEmailContext emailContext)
        {
            Check.If(emailContext).IsNotNull();

            _emailContext = emailContext;
        }

        public Email GetEmailByReference(string emailReference)
        {
            return emailReference.IsNullOrEmpty()
                ? null
                : _emailContext.Emails.FirstOrDefault(x => x.EmailReference == emailReference);
        }

        public bool CreateEmail(Email email)
        {
            if (email.EmailReference.IsNullOrEmpty())
                return false;

            _emailContext.Emails.Add(email);

            return _emailContext.SaveChanges() > 0;
        }

        public bool UpdateStatus(string emailReference, Status status)
        {
            if (emailReference.IsNullOrEmpty())
                return false;

            var email = GetEmailByReference(emailReference);

            if (email.IsNull())
                return false;

            email.Status = status;

            return _emailContext.SaveChanges() > 0;
        }

        public bool UpdateAppliedRules(string emailReference, string ruleApplied)
        {
            if (emailReference.IsNullOrEmpty())
                return false;

            var email = GetEmailByReference(emailReference);

            if (email.IsNull())
                return false;

            email.AppliedRules.Add(new AppliedRule {RuleName = ruleApplied});

            return _emailContext.SaveChanges() > 0;
        }
    }
}
