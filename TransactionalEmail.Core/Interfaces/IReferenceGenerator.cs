﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalEmail.Core.Interfaces
{
    public interface IReferenceGenerator
    {
        string CreateReference(int size);
    }
}
