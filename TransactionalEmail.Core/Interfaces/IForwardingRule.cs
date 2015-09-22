using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Interfaces
{
    public interface IForwardingRule
    {
        string RuleName { get; }
        RuleResult ApplyRule(Email email);
    }
}
