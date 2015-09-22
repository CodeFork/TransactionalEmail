namespace TransactionalEmail.Core.Objects
{
    public class RuleResult
    {
        public string RuleName { get; set; }
        public bool RuleApplied { get; set; }
        public Email Email { get; set; }
    }
}
