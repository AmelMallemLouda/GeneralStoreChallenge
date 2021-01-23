﻿using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        [HttpPost]
        public async Task<IHttpActionResult> CreateProduct([FromBody] Product product)
        {
            if (product is null)
            {
                return BadRequest("the Product does not exist");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return Ok("The product was successfully created");
            }
            return BadRequest(ModelState);
        }

        [HttpGet]

        public async Task<IHttpActionResult> GetallProducts()
        {
            List<Product> products = await _context.Products.ToListAsync();
                return Ok(products);
        }
        [HttpGet]

        public async Task<IHttpActionResult> GetProductById([FromUri] string sku)
        {
            Product product = await _context.Products.FindAsync(sku);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }
        [HttpPut]
        public async Task<IHttpActionResult> Updateproduct([FromUri] string sku,[FromBody] Product updatedProduct) 
        {
            //Chech the Ids if they match
            if(sku != (updatedProduct?.SKU))
            {
                return BadRequest("Ids do not mutch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Find the product
            Product product = await _context.Products.FindAsync(sku);

            //if the restaurant does not exist then do something 
            if (product is null)
                return NotFound();
            //Update The product
            product.Name = updatedProduct.Name;
            product.Cost = updatedProduct.Cost;
            product.NumberInventory = updatedProduct.NumberInventory;

            //save changes

           await _context.SaveChangesAsync();
            return Ok("The product was successfully updated");

        }
        [HttpDelete]

        public async Task<IHttpActionResult> DeleteProduct([FromUri]string sku)
        {
            //Find the Product
            Product product = await _context.Products.FindAsync(sku);
            if (product is null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            if(await _context.SaveChangesAsync()==1)
            {
                return Ok("The product was successfully deleted");
            }
            return InternalServerError();

                    

            
        }
    }
}
