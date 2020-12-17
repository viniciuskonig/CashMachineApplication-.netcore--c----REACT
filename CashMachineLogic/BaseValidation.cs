using CashMachineModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashMachineLogic
{
    public class BaseValidation 
    {
        public void ValidateCoinValueLessThenZero(Coin coin)
        {
            if (coin.Value < 0)
            {
                throw new CoinValueLessThanZeroException();
            }
        }

        public void ValidadeQuantityLessThenZero(Coin coin)
        {
            if (coin.Quantity < 0)
            {
                throw new CoinQuantityLessThanZeroException();
            }
        }


        public void ValidateForChangeBalance(decimal requiredAmount)
        {
            ICoinLogic logic = new CoinLogic();
            var total = logic.GetBalance();
            if (requiredAmount > total)
            {
                throw new NotEnoughBalanceForChangeException();
            }
        }

        public void ValidateDuplicatedCoin(Coin coin)
        {
            ICoinLogic logic = new CoinLogic();
            if (logic.GetCoins().Any(o => o.Value == coin.Value))
            {
                throw new DuplicatedCoinException();
            }
        }

        /// <summary>
        /// Uses the UpdatedOn prop to check if the data were not changed before (check if has last version)
        /// </summary>
        /// <param name="coin"></param>
        /// <param name="currentState"></param>
        public void ValidadeIsCurrentVersion(Coin coin, List<Coin> currentState)
        {
            if (currentState.Any(x => (x.Value == coin.Value && x.UpdatedOn != coin.UpdatedOn)))
            {
                throw new OutdatedVersionOfCoinException();
            }
        }
    }
}
