using CashMachineModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashMachineRepository
{
    public class CoinRepository: ICoinRepository
    {
        static List<Coin> initialState = new List<Coin>() { };
        static List<Coin> currentState = initialState;

        public static List<Coin> InitialState
        {
            get { return initialState; } set { }
        }

        public List<Coin> CurrentState
        {
            get { return currentState; } set { currentState = value; }
        }

        public List<Coin> GetCoins()
        {
            return this.CurrentState;
        }

        public decimal GetTotal()
        {
            return this.CurrentState.Select(x => x.Quantity * x.Value).Sum();
        }

        public void ResetBalance()
        {
            this.CurrentState.Clear();
        }

        public void UpdateCoin(Coin coin)
        {
            this.CurrentState.
            Where(x => x.Value == coin.Value)
            .Select(x =>
            {
                x.Quantity = coin.Quantity;
                x.UpdatedOn = DateTime.Now;
                return x;
            })
            .ToList();
        }
    }
}
