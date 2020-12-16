using System;
using System.Collections.Generic;
using System.Text;

namespace CashMachineLogic
{
    
    public class DuplicatedCoinException : Exception
    {
        public DuplicatedCoinException() : base("Ops! this coin already exists.") { }
    }
}
