using System;
using System.Collections.Generic;
using System.Text;

namespace CashMachineLogic
{
    
    public class CoinQuantityLessThanZeroException : Exception
    {
        public CoinQuantityLessThanZeroException() : base("Quantity needs to be higher than 0.") { }
    }
}
