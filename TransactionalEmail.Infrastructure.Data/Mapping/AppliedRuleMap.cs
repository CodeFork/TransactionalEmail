using System.Data.Entity.ModelConfiguration;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Infrastructure.Data.Mapping
{
    public class AppliedRuleMap : EntityTypeConfiguration<AppliedRule>
    {
        public AppliedRuleMap()
        {
            Property(x => x.RuleName)
                .HasMaxLength(255);
        }
    }
}
