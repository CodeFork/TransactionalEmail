using System;

namespace TransactionalEmail.Core.Objects
{
    public class AppliedRule
    {
        public int AppliedRuleId { get; set; }
        public string RuleName { get; set; }
        public DateTime DateCreated { get; set; }

        public AppliedRule()
        {
            RuleName = string.Empty;
            DateCreated = DateTime.UtcNow;
        }
    }
}
