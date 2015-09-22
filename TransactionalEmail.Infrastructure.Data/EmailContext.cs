﻿using System.Data.Entity;
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
    }
}
