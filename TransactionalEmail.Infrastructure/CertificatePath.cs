using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Infrastructure
{
    public class CertificatePath : ICertificatePath
    {
        public string Value { get; set; }
    }
}
