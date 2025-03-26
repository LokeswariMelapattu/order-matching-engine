using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMatchingEngine.Domain.Enums
{
    public enum OrderStatus 
    { 
        New, 
        PartiallyFilled, 
        Filled, 
        Cancelled 
    }
}
