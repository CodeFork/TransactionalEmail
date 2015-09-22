using System.Data.Entity;
using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Infrastructure.Data
{
    public class EmailContext : DbContext, IEmailContext
    {
        public EmailContext(IDbSettings connectionSettings)
            : base(connectionSettings.ConnectionString)
        {
            Check.If(connectionSettings).IsNotNull();
            Check.If(connectionSettings.ConnectionString).IsNotNullOrEmpty();

            Configuration.LazyLoadingEnabled = false;
        }

        public EmailContext()
            : base(Constants.DefaultDatabaseName)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Email> Emails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Email>()
                .HasMany(e => e.EmailAddresses)
                .WithOptional()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Email>()
                .HasMany(a => a.Attachments)
                .WithOptional()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Email>()
                .HasMany(r => r.AppliedRules)
                .WithOptional()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Email>()
                .Ignore(x => x.FromAddress);

            modelBuilder.Entity<Email>()
                .Ignore(x => x.ToAddresses);

            modelBuilder.Entity<Email>()
                .Ignore(x => x.Ccs);

            modelBuilder.Entity<Email>()
                .Ignore(x => x.Bccs);
        }
    }
}
