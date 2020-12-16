using CashMachineLogic;
using CashMachineModel;
using System.Collections.Generic;

public class CashMachineServices : ICashMachineServices
{
    public List<Coin> GiveChange(int requiredAmount)
    {
        var process = new ProcessGiveChange();
        var coin = process.GiveChange(requiredAmount, new List<Coin>());

        return coin;
    }

    public List<Coin> GetCoins()
    {
        ICoinLogic logic = new CoinLogic();
        var coins = logic.GetCoins();

        return coins;
    }
}
