using CashMachineModel;
using System.Collections.Generic;

namespace CashMachineRepository
{
    public static class CoinRepository
    {
        static List<Coin> initialState = new List<Coin>() { };
        static List<Coin> currentState = initialState;

        public static List<Coin> InitialState
        {
            get { return initialState; } set { }
        }

        public static List<Coin> CurrentState
        {
            get { return currentState; } set { currentState = value; }
        }
    }
}
