using System.Collections.Generic;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Interfaces
{
    public interface IEmailRepository
    {
        Email GetEmailByReference(string emailReference);
        bool CreateEmail(Email email);
        bool UpdateStatus(string emailReference, Status status);
        bool UpdateAppliedRules(string emailReference, string ruleApplied);
    }
}
