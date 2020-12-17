using System;
using System.Collections.Generic;
using System.Text;

namespace CashMachineLogic
{
    
    public class CoinValueLessThanZeroException : Exception
    {
        public CoinValueLessThanZeroException() : base("Value needs to be higher than 0.") { }
    }
}
