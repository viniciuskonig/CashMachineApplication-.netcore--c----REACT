using CashMachineModel;
using System.Collections.Generic;

namespace CashMachineLogic
{
    public interface ICoinLogic
    {

        /// <summary>
        /// Add a new coin
        /// </summary>
        /// <param name=""></param>
        void AddCoin(Coin coin);

        /// <summary>
        /// Returns Total Balance
        /// </summary>
        /// <returns></returns>
        decimal GetBalance();

        /// <summary>
        /// Return List of coins
        /// </summary>
        /// <returns></returns>
        List<Coin> GetCoins();

        /// <summary>
        /// Update coin value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        void UpdateCoin(Coin coin);

        /// <summary>
        /// Gives change, reducing from current balance
        /// </summary>
        void GiveChange(decimal requiredAmount);

        /// <summary>
        /// Resets Balance
        /// </summary>
        void ResetBalance();
    }
}