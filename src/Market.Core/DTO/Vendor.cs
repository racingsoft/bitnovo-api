﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.DTO
{
    public class Vendor : IVendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
