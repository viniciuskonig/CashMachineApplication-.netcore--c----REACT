using AutoMapper;
using CashMachineApp.Models;
using CashMachineModel;

namespace CashMachineApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Coin, CoinModel>();
            CreateMap<CoinModel, Coin>();
        }
    }
}
