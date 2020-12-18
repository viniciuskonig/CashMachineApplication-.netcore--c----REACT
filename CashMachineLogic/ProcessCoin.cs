using CashMachineModel;
using System;
using System.Linq;

namespace CashMachineLogic
{
    public class ProcessCoin : BaseValidation
    {
        /// <summary>
        /// the coin to process
        /// </summary>
        private Coin CoinToProcess { get; set; }

        /// <summary>
        /// Amount to remove
        /// </summary>
        private decimal RequiredAmount { get; set; }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="coinToProcess">the coin value</param>
        /// <param name="requiredAmount">the amount to get</param>
        public ProcessCoin(Coin coinToProcess, decimal requiredAmount)
        {
            CoinToProcess = coinToProcess;
            RequiredAmount = requiredAmount;
        }

        /// <summary>
        /// Get the highest amout of coins to give as change
        /// </summary>
        /// <returns></returns>
        public int CalculateQuantityForChange(CoinLogic logic)
        {
            try
            {
                var coins = logic.GetCoins();
                var coinToProcess = coins.Where(x => x.Value == CoinToProcess.Value).SingleOrDefault();
                var quantity = (RequiredAmount / CoinToProcess.Value);

                //Validade if value is not higher than currently have
                if (quantity > coinToProcess.Quantity)
                {
                    quantity = coinToProcess.Quantity;
                }

                return (int)quantity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
