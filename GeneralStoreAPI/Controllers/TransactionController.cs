using GeneralStoreAPI.Models;
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
    public class TransactionController : ApiController
    {
        private readonly Transaction transactions = new Transaction();
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        [HttpPost]
        public async Task<IHttpActionResult> Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                //Find Product and Customer
                Product product = await _context.Products.FindAsync(transaction.ProductSKU.ToString());
                Customer customer = await _context.Customers.FindAsync(transaction.CustomerId);
                //Verify that the product is in stock
                if (transaction.Product.IsInStock)
                {
                    //Check that there is enough product to complete the Transaction
                    if (transaction.ItemCount <= transaction.Product.NumberInventory)
                    {
                        //add transaction
                        _context.Transactions.Add(transaction);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest($"There are only {transaction.Product.NumberInventory} of {transaction.ItemCount}left in stock");
                }


                //Remove the Products that were bought
                transaction.Product.NumberInventory -= transaction.ItemCount;//decrementation of the product.NumberInventory according to transaction.ItemCount

               
            }
            return BadRequest(ModelState);   
        }
        // Get All
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Transaction> transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }


        //[HttpGet]
        //public async Task<IHttpActionResult> GetAll
    }
}
