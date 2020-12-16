using System;
using System.Collections.Generic;
using System.Text;

namespace CashMachineLogic
{
    public class NotEnougthQuantityForChangeException : Exception
    {
        public NotEnougthQuantityForChangeException() : base("There was not enough quantity of coins.") { }
    }
}
