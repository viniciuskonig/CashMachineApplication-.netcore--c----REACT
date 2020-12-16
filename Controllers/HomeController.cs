using System;
using System.Collections.Generic;
using AutoMapper;
using CashMachineApp.Models;
using CashMachineLogic;
using CashMachineModel;
using Microsoft.AspNetCore.Mvc;

namespace CashMachineApp.Controllers
{
    public class HomeController : Controller
    {
        private static List<CoinModel> _coins;

        private readonly IMapper _mapper;
        private readonly ICoinLogic _coinLogic;

        public HomeController(IMapper mapper, ICoinLogic coinLogic)
        {
            _mapper = mapper;
            _coinLogic = coinLogic;
            _coins = _mapper.Map<List<CoinModel>>(_coinLogic.GetCoins());
        }

        public IActionResult Index()
        {
            return View(_coins);
        }

        [HttpGet]
        public IActionResult GetCoins()
        {
            try
            {
                _coins = _mapper.Map<List<CoinModel>>(_coinLogic.GetCoins());

                return Json(new Result() { Message = "Coins loaded with success /:)", Data = _coins, Error = false });
            }
            catch (Exception ex)
            {
                return Json(new Result() { Message = ex.Message, Error = true });
            }
        }

        [HttpPost]
        public IActionResult UpdateBalance([FromBody] List<Coin> coins)
        {
            try
            {
                coins.ForEach(x => _coinLogic.UpdateCoin(x));
                return Json(new Result() { Message = "Balance updated!" });
            }
            catch (Exception ex)
            {
                return Json(new Result() { Message = ex.Message, Error = true });
            }
        }

        [HttpPost]
        public IActionResult AddNewCoin([FromBody] Coin coin)
        {
            try
            {
                _coinLogic.AddCoin(coin);
                return Json(new Result() { Message = "Looks we got a new Coin :)" });
            }
            catch (Exception ex)
            {
                return Json(new Result() { Message = ex.Message, Error = true });
            }
        }

        [HttpPost]
        public IActionResult GiveChange([FromBody] decimal requestedValue)
        {
            try
            {
                _coinLogic.GiveChange(requestedValue);
                return Json(new Result() { Message = "Change given!", Error = false });
            }
            catch (Exception ex)
            {
                return Json(new Result() { Message = ex.Message, Error = true });
            }
        }
    }

    public class Result {
        public dynamic Data { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }
    }
}