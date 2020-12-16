using System;

namespace CashMachineApp.Models
{
    public class CoinModel
    {
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}