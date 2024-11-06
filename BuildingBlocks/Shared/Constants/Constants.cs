﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Constants
{
    public class Constants
    {
        public class Role
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }
        public class OrderStatus 
        {
            public const string Pending = "Pending";
            public const string Paid = "Paid";
            public const string Cancel = "Cancel";
        }
    }
}
