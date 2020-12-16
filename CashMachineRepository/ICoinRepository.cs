using CashMachineModel;
using System.Collections.Generic;
namespace AtmRepository
{
    public interface ICoinRepository
    {
        List<Coin> GetCoins();
    }
}