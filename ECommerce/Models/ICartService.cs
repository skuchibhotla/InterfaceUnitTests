﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ECommerce.Models.Models;

namespace ECommerce.Models
{
    public interface ICartService
    {
        double Total();
        IEnumerable<CartItem> Items();
    }
}
