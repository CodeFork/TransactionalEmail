using System.Collections.Generic;

namespace TransactionalEmail.Core.Objects
{
    public class ForwardResult
    {
        public string RuleApplied { get; set; }
        public Email EmailResult { get; set; }       
    }
}
