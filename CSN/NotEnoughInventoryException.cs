using System;
using System.Collections.Generic;

namespace CSN
{
    public class NotEnoughInventoryException : Exception
    {
        public List<NameQuantity> Missing { get; set; }
    }
}
