using System.Data.Entity.ModelConfiguration;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Infrastructure.Data.Mapping
{
    public class AttachmentMap : EntityTypeConfiguration<Attachment>
    {
        public AttachmentMap()
        {
            Property(x => x.AttachmentName)
                .HasMaxLength(255);

            Property(x => x.MimeType)
                .HasMaxLength(255);
        }
    }
}
