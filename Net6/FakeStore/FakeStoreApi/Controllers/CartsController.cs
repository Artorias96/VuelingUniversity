﻿using Business.ServiceContracts;
using Domain.DomainEntities.CartEntities;
using Domain.DomainEntities.ProductEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartsController> _logger;

        private const string errorMsg = "Error trying to search data";
        public CartsController(ICartService cartService, ILogger<CartsController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }
        /// <summary>
        /// Show All the carts from the FakeStore Url
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDefaultListCarts")]
        public IActionResult ShowAllCartsFromUrl()
        {
            try
            {
                CartInfoList cartsList = _cartService.GetCarts();
                _logger.LogInformation("The information has been read successfully");

                return Ok(cartsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(errorMsg);
                return BadRequest($"Some error ocurred {ex.Message}");
            }
        }

        [HttpPost]
        [Route("GetCartPrice")]
        public IActionResult GetCostCartByIdCart(int id)
        {
            try
            {
                float totalMoneyFromCartProduct = _cartService.GetPriceCart(id);
                return Ok($"The Cost from the products of cart with ID {id} is {totalMoneyFromCartProduct}");
            }
            catch (Exception ex)
            {
                _logger.LogError(errorMsg);
                return StatusCode(500,$"Some error ocurred while processing your request");
            }
        }
    }
}
