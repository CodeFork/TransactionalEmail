using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Infrastructure.Data
{
    public interface IEmailContext
    {
        DbSet<Email> Emails { get; set; }
        int SaveChanges();
    }
}
