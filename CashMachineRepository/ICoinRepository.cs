using CashMachineModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashMachineRepository
{
    public interface ICoinRepository
    {
        public decimal GetTotal();

        public List<Coin> GetCoins();

        public void ResetBalance();

        void UpdateCoin(Coin coin);
    }
}
