using System.Data.Entity.ModelConfiguration;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Infrastructure.Data.Mapping
{
    public class EmailMap : EntityTypeConfiguration<Email>
    {
        public EmailMap()
        {
            HasMany(e => e.EmailAddresses)
                .WithOptional()
                .WillCascadeOnDelete(true);

            HasMany(a => a.Attachments)
                .WithOptional()
                .WillCascadeOnDelete(true);

            HasMany(r => r.AppliedRules)
                .WithOptional()
                .WillCascadeOnDelete(true);

            Ignore(x => x.FromAddress);

            Ignore(x => x.ToAddresses);

            Ignore(x => x.Ccs);

            Ignore(x => x.Bccs);

            Property(x => x.EmailReference)
                .HasMaxLength(25);

            Property(x => x.AccountName)
                .HasMaxLength(255);
        }
    }
}
