using CashMachineLogic;
using CashMachineModel;
using NUnit.Framework;
using System;
using System.Linq;

namespace CashMachineTests
{

    [TestFixture]
    public class CoinLogicTest
    {
        [SetUp]
        public void SetupBeforeEachTest()
        {
            ICoinLogic logic = new CoinLogic();
            logic.ResetBalance();
        }

        /// <summary>
        /// Test AddCoin (Sucess)
        /// </summary>
        [Test]
        public void AddCoinTest()
        {
            ICoinLogic logic = new CoinLogic();
            logic.AddCoin(new Coin() { Value = 2, Quantity = 2 });
            logic.AddCoin(new Coin() { Value = 1, Quantity = 2 });
            Assert.AreEqual(2, logic.GetCoins().Count);
        }

        /// <summary>
        /// Test AddCoin with a coin that has already the new coin's value (Fail)
        /// </summary>
        [Test]
        public void AddCoinDuplicatedValueTest()
        {
            ICoinLogic logic = new CoinLogic();
            logic.AddCoin(new Coin() { Value = 1, Quantity = 2 });

            Assert.Throws<DuplicatedCoinException>(() => logic.AddCoin(new Coin() { Value = 1, Quantity = 2 }));

        }

        /// <summary>
        /// Test AddCoin with a coin that value is Zero (Fail)
        /// </summary>
        [Test]
        public void AddCoinWithValueLessThanZeroValueTest()
        {
            ICoinLogic logic = new CoinLogic();
            
            Assert.Throws<CoinValueLessThanZeroException>(() => logic.AddCoin(new Coin() { Value = -1, Quantity = 2 }));
        }

        /// <summary>
        /// Test AddCoin with a coin that value is Zero (Fail)
        /// </summary>
        [Test]
        public void AddCoinWithQuantityLessThanZeroValueTest()
        {
            ICoinLogic logic = new CoinLogic();

            Assert.Throws<CoinQuantityLessThanZeroException>(() => logic.AddCoin(new Coin() { Value = 1, Quantity = -1 }));
        }

        /// <summary>
        /// Test Update coin (Sucess)
        /// </summary>
        [Test]
        public void UpdateCoinTest()
        {
            ICoinLogic logic = new CoinLogic();
            logic.AddCoin(new Coin() { Value = 1, Quantity = 2 });

            var coinToBeUpdated = logic.GetCoins().Where(o => o.Value == 1).FirstOrDefault();
            coinToBeUpdated.Quantity = 3;
            coinToBeUpdated.UpdatedOn = DateTime.Now;
            logic.UpdateCoin(coinToBeUpdated);

            //Gets updated coin
            var coinUpdated = logic.GetCoins().Where(o => o.Value == 1).FirstOrDefault();

            Assert.AreEqual(coinUpdated.Quantity, 3);
        }

        /// <summary>
        /// Test Update coin on an updated record (Fail)
        /// </summary>
        [Test]
        public void UpdateCoinOutdatedTest()
        {
            ICoinLogic logic = new CoinLogic();
            logic.AddCoin(new Coin() { Value = 1, Quantity = 2 });

            var coinToBeUpdated = logic.GetCoins().Where(o => o.Value == 1).FirstOrDefault();
            coinToBeUpdated.Quantity = 3;
            coinToBeUpdated.UpdatedOn = DateTime.Now;
            logic.UpdateCoin(coinToBeUpdated);

            //Gets updated coin
            var coinUpdatedOutdated = new Coin();
            coinUpdatedOutdated.Value = coinToBeUpdated.Value;
            coinUpdatedOutdated.Quantity = 3;
            coinUpdatedOutdated.UpdatedOn = DateTime.Now.AddDays(-1);

            Assert.Throws<OutdatedVersionOfCoinException>(() => logic.UpdateCoin(coinUpdatedOutdated));
        }

        /// <summary>
        /// Test Get balance
        /// </summary>
        [Test]
        public void GetBalanceTest()
        {
            ICoinLogic logic = new CoinLogic();
            logic.AddCoin(new Coin() { Value = 1, Quantity = 5 });
            logic.AddCoin(new Coin() { Value = 2, Quantity = 5 });
            var total = logic.GetBalance();
            Assert.AreEqual(15, total);
        }

        /// <summary>
        /// Test give change to go to 0 balance
        /// </summary>
        [Test]
        public void GiveChangeToGoToZeroBalanceTest()
        {
            //Add balance
            ICoinLogic logic = new CoinLogic();
            logic.AddCoin(new Coin() { Value = 0.5m, Quantity = 1 });
            logic.AddCoin(new Coin() { Value = 1, Quantity = 1 });
            logic.AddCoin(new Coin() { Value = 2, Quantity = 1 });
            var currentCoins = logic.GetCoins();

            //Request Change
            logic.GiveChange(3.5m);

            //Reload coins
            currentCoins = logic.GetCoins();

            //Vefifies if coins are now ZERO 
            var coins = logic.GetCoins();

            Assert.AreEqual(0, coins.Sum(x => x.Quantity * x.Value));
        }

        /// <summary>
        /// Test Exception for current balance < requested change
        /// </summary>
        [Test]
        public void GiveMoreChangeThenCurrentBalanceTest()
        {
            ICoinLogic logic = new CoinLogic();
            logic.AddCoin(new Coin() { Value = 2, Quantity = 1 });
            Assert.Throws<NotEnoughBalanceForChangeException>(() => logic.GiveChange(3m));
        }


        /// <summary>
        /// Test Exception for current quantity of coins < requested change
        /// </summary>
        [Test]
        public void GiveMoreChangeThenCurrentQuantityTest()
        {
            ICoinLogic logic = new CoinLogic();
            logic.AddCoin(new Coin() { Value = 2, Quantity = 2 });
            logic.AddCoin(new Coin() { Value = 1, Quantity = 1 });
            Assert.Throws<NotEnougthQuantityForChangeException>(() => logic.GiveChange(2.3m));
        }

        /// <summary>
        /// Test give change to go to 0 balance
        /// </summary>
        [Test]
        public void GiveChangeEvenIfLowerIsntThenHighestAmountQuantityTest()
        {
            //Add balance
            ICoinLogic logic = new CoinLogic();
            logic.AddCoin(new Coin() { Value = 0.10m, Quantity = 10 });
            logic.AddCoin(new Coin() { Value = 0.13m, Quantity = 2 });
            logic.AddCoin(new Coin() { Value = 0.25m, Quantity = 1 });
            var currentCoins = logic.GetCoins();

            //Request Change
            logic.GiveChange(0.45m);

            //Reload coins
            currentCoins = logic.GetCoins();

            //Vefifies if coins are now ZERO 
            var coins = logic.GetCoins();

            Assert.AreEqual(0, 0);
        }
    }
}
