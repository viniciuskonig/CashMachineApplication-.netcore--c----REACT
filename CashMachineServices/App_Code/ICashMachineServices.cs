using System.Collections.Generic;
using CashMachineModel;

[ServiceContract]
public interface ICashMachineServices
{
    [OperationContract]
    List<Coin> GiveChange(int requiredAmount);

    [OperationContract]
    List<Coin> GetCoins();
}
