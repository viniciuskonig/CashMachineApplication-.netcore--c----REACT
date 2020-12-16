using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CashMachineModel
{
    [Serializable]
    public class Coin
    {
        public int Quantity { get; set; }

        public decimal Value { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
