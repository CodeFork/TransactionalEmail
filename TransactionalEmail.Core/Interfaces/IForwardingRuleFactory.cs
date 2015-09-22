using System.Collections.Generic;

namespace TransactionalEmail.Core.Interfaces
{
    public interface IForwardingRuleFactory
    {
        List<IForwardingRule> GetRules();
    }
}
