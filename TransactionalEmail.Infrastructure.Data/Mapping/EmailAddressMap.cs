using System.Data.Entity.ModelConfiguration;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Infrastructure.Data.Mapping
{
    public class EmailAddressMap : EntityTypeConfiguration<EmailAddress>
    {
        public EmailAddressMap()
        {
            Property(x => x.Name)
                .HasMaxLength(50);

            Property(x => x.Email)
                .HasMaxLength(255);
        }
    }
}
