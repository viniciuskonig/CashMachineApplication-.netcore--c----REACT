using System;
using System.Collections.Generic;
using System.Text;

namespace CashMachineLogic
{
    public class NotEnoughBalanceForChangeException : Exception
    {
        public NotEnoughBalanceForChangeException() : base("There was not enough quantity of coins.") { }
    }
}
