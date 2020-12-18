using CashMachineModel;
using CashMachineRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashMachineLogic
{
    public class CoinLogic : BaseValidation, ICoinLogic
    {
        private readonly ICoinRepository _coinRepository;

        public CoinLogic(ICoinRepository coinRepository)
        {
            _coinRepository = coinRepository;
        }

        /// <summary>
        /// Method to add new coins
        /// </summary>
        /// <param name="coin">coin to be added</param>
        public void AddCoin(Coin coin)
        {
            try
            {
                base.ValidateCoinValueLessThenZero(coin);
                base.ValidadeQuantityLessThenZero(coin);
                base.ValidateDuplicatedCoin(coin, this);

                var currentState = _coinRepository.GetCoins();
                currentState.Add(coin);
            }
            catch (Exception ex)
            {
                if (ex is DuplicatedCoinException || ex is CoinValueLessThanZeroException || ex is CoinQuantityLessThanZeroException)
                {
                    throw ex;
                }

                throw new Exception("An error occoured. It was not possible to add your coin :(.");
            }
        }

        /// <summary>
        /// Returns Total Balance
        /// </summary>
        /// <returns></returns>
        public decimal GetBalance()
        {
            try
            {
                //var total = 
                var total = _coinRepository.GetTotal();
                return total;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Return List of coins
        /// </summary>
        /// <returns></returns>
        public List<Coin> GetCoins()
        {
            try
            {
                var coin = _coinRepository.GetCoins().OrderBy(o => o.Value).ToList();
                return coin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates coin value
        /// </summary>
        /// <param name="coin"></param>
        /// <param name="checkVersion"></param>
        /// <returns></returns>
        public void UpdateCoin(Coin coin)
        {
            try
            {
                var currentState = _coinRepository.GetCoins();

                base.ValidateCoinValueLessThenZero(coin);
                base.ValidadeQuantityLessThenZero(coin);

                //Checks if the coin is not being overwritten by old state
                base.ValidadeIsCurrentVersion(coin, currentState);

                _coinRepository.UpdateCoin(coin);
            }
            catch (Exception ex)
            {
                if (ex is OutdatedVersionOfCoinException)
                {
                    throw new OutdatedVersionOfCoinException();
                }

                throw new Exception("An error occoured. It was not possible to update your coin :(.");
            }
        }

        /// <summary>
        /// Uses the minimun amount of coins (by its value) to give as change; 
        /// this will update the coin's state
        /// </summary>
        /// <param name="requiredAmount"></param>
        public void GiveChange(decimal requiredAmount)
        {
            try
            {
                base.ValidateForChangeBalance(requiredAmount, this);

                var coins = _coinRepository.GetCoins().OrderByDescending(o => o.Value).ToList();
                var coinsForSimulation = DeepCloneHelper.DeepClone(coins.Select(c => c).ToList());
                decimal amountRemainingForSimulation = DeepCloneHelper.DeepClone(requiredAmount);

                //Uses de deepclone to smulate first if there is no amount remaning
                amountRemainingForSimulation = ProcessGiveChange(amountRemainingForSimulation, coinsForSimulation, false);

                if (amountRemainingForSimulation != 0)
                {
                    throw new NotEnougthQuantityForChangeException();
                }
                else
                {
                    //Process the change in the coin state (update values/remove quantities)
                    ProcessGiveChange(requiredAmount, coins, true);
                }
            }
            catch (Exception ex)
            {
                if (ex is NotEnougthQuantityForChangeException || ex is NotEnoughBalanceForChangeException)
                {
                    throw ex;
                }

                throw new Exception("An error occoured. It was not possible to give you change :(."); ;
            }
        }

        /// <summary>
        /// Process the coins to give change
        /// </summary>
        /// <param name="requireAmount">requireAmount to subtract from the requ</param>
        /// <param name="coins"></param>
        /// <returns></returns>
        private decimal ProcessGiveChange(decimal requiredAmount, List<Coin> coins, bool updateCoin = false)
        {
            var remainingValue = requiredAmount;
            //Last coin used
            var coinAux = new Coin();

            //Go through the parameter coin list, uses the CalculateQuantityForChange, it will return the amount of quantities
            //to be reduced from the current coin, if updateCoin is true: will update State (reducing the quantity of the coin utilized for give the change)
            //next it will populate the coinAux, which is used if the remaning amount is less than 0 
            coins
                .Select((c, index) =>
                {
                    if (remainingValue != 0)
                    {
                        var coin = c;
                        var quantity = new ProcessCoin(coin, remainingValue).CalculateQuantityForChange(this);

                        remainingValue -= coin.Value * quantity;

                        c.Quantity -= quantity;

                        if (updateCoin)
                        {
                            this.UpdateCoin(c);
                        };

                        if (quantity != 0)
                        {
                            coinAux = c;
                        }
                    }

                    return c;
                }).Where(r => r.Quantity > 0).ToList();

            //Now, if the remaning amount is not zero, that means the coin that was 
            //last utilized COULD make the remaningValue impossible for lower values
            //then we utilize the lastCoin, giving back one quantity
            //if the state was change (updateCoin is true) we call the update to give back one quantity
            //in the end we call the ProcessGiveChange over and over again trying lower coins
            if (remainingValue != 0)
            {
                var lastCoin = coinAux;
                lastCoin.Quantity = lastCoin.Quantity + 1;

                //Rollback last coin changed (giving back 1)
                if (updateCoin)
                {
                    this.UpdateCoin(lastCoin);
                }

                //Skip last used coin, in order to get the remaning value
                var lowerCoins = coins.Where(o => o.Value < lastCoin.Value).OrderByDescending(o => o.Value).ToList();
                if (lowerCoins.Count > 0)
                {
                    //remainingValue was deduced from last interation (see coins.Select() above) so we will be using it + lastCoin.Value
                    return ProcessGiveChange(remainingValue + lastCoin.Value, lowerCoins, updateCoin);
                }
            }

            return remainingValue;
        }

        public void ResetBalance()
        {
            _coinRepository.ResetBalance();
        }
    }
}
