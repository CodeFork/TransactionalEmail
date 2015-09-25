using System.Data.Entity;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Infrastructure.Data
{
    public interface IEmailContext
    {
        DbSet<Email> Emails { get; set; }
        int SaveChanges();
    }
}
