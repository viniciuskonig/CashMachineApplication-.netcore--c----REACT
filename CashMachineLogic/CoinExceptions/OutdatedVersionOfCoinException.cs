using System;
using System.Collections.Generic;
using System.Text;

namespace CashMachineLogic
{
    
    public class OutdatedVersionOfCoinException : Exception
    {
        public OutdatedVersionOfCoinException() : base("Values were overwritten. No changes were made.") { }
    }
}
