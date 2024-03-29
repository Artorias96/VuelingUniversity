﻿using Business.Dtos;
using Business.ServiceContracts;
using Crosscutting.CustomExceptions;
using Domain.DomainEntities.ProductEntities;
using FakeStoreApi.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FakeStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        private const string errorMsg = "Error trying to search data";
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// This Method show All the products from the Api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDefaultListProducts")]
        public IActionResult ShowAllProducts()
        {
            try
            {
                ProductInfoList productsList = _productService.GetProducts();
                _logger.LogInformation("The information from products has been read successfully");

                return Ok(productsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(errorMsg);
                return BadRequest($"Some error ocurred {ex.Message}");
            }
        }

        /// <summary>
        /// This method Show the actual products if some product was added, deleted or updated
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetActualListProducts")]
        public IActionResult GetActualListProduct()
        {
            try
            {
                ProductInfoList productsList = _productService.GetActualProducts();
                _logger.LogInformation("The actual information of products has been read successfully");

                return Ok(productsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(errorMsg);
                return BadRequest($"Some error ocurred {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new Product
        /// </summary>
        /// <param name="productToInsert"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetNewProduct")]
        public IActionResult CreateNewProduct(ProductModel productToInsert)
        {
            try
            {
                Product productCreated = new Product
                {
                    id = productToInsert.id,
                    category = productToInsert.category,
                    description = productToInsert.description,
                    price = productToInsert.price,
                };

                string reponse = _productService.CreateNewProduct(productCreated);

                _logger.LogInformation("The Product has been inserted successfully");
                return Ok(reponse);
            }
            catch (AlreadyExistingProduct ex)
            {
                _logger.LogError(errorMsg);
                return BadRequest($"Some error ocurred trying to insert product with Id {ex.Message}, the product already exist");
            }
        }

        /// <summary>
        /// Delete a Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteProduct")]
        public IActionResult DeleteProductById(int id)
        {
            try
            {
                int idProductDeleted = _productService.DeleteProductById(id);
                _logger.LogInformation("The Product has been removed successfully");
                return Ok($"The product with Id {idProductDeleted} was succesfully removed");
            }
            catch (NotExistingProduct ex)
            {
                _logger.LogError(errorMsg);
                return BadRequest($"Some error ocurred trying to delete product with Id {ex.Message}, the product does not exist");
            }
        }
        /// <summary>
        /// Get the product by the product ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductSelectedByID")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                Product productSelected = _productService.SelectProductById(id);
                ProductDTO productSelectedAsDto = new ProductDTO
                {
                    category = productSelected.category,
                    description = productSelected.description,
                    price = productSelected.price,
                };

                _logger.LogInformation("The Product has been found successfully");
                return Ok(productSelectedAsDto);
            }
            catch (NotExistingProduct ex)
            {
                _logger.LogError(errorMsg);
                return BadRequest($"Some error ocurred trying to get product with Id {ex.Message}, the product does not exist");
            }
        }

        /// <summary>
        /// Update Price of the product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPrice"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdatePriceProduct")]
        public IActionResult UpdateProductSelectById(int id, float newPrice)
        {
            try
            {
                int idProductUpdated = _productService.UpdatePriceProductById(id, newPrice);
                _logger.LogInformation("The Product has been Updated successfully");
                return Ok($"Changes in product with Id {idProductUpdated} was succesfully update");
            }
            catch (NotExistingProduct ex)
            {
                _logger.LogError(errorMsg);
                return BadRequest($"Some error ocurred trying to update product with Id {ex.Message}, the product does not exist");
            }
        }
    }
}
