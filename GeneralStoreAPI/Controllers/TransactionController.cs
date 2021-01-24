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
       // private readonly Transaction transaction = new Transaction();
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        [HttpPost]
        public async Task<IHttpActionResult> Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                Product product = await _context.Products.FindAsync(transaction.ProductSKU);
                Customer customer = await _context.Customers.FindAsync(transaction.CustomerId);

                if (product == null)
                {
                    return BadRequest("invalid Product ID");
                }

                if (customer == null)
                {
                    return BadRequest("invalid customer ID");
                }

                if (transaction.ItemCount > product.NumberInventory)
                {
                    return BadRequest($"There are only {product.NumberInventory} of {product.Name} left in stock");
                }
                product.NumberInventory -= transaction.ItemCount;

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return Ok("Your Transaction was successfully created");
            }
            return BadRequest(ModelState);   // 400
        }

        // Get All
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Transaction> transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetTransactionById([FromUri] int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);
            if(transaction is null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
        [HttpPut]
        public async Task<IHttpActionResult> UpdateTransaction([FromUri]int id,[FromBody] Transaction updatedTransaction)
        {
            //Chech the Ids if they match
            if(id != updatedTransaction?.Id)
            {
                return BadRequest("The Ids do not match");
            }

            //Check the modelState
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Find the Transaction in the database
            Transaction transaction = await _context.Transactions.FindAsync(id);

            //if the transaction does not exist then do something 
            if(transaction is null)
            {
                return NotFound();
            }

            //Update the Propreties 
            transaction.Id = updatedTransaction.Id;
            transaction.ItemCount = updatedTransaction.ItemCount;
            transaction.ProductSKU = updatedTransaction.ProductSKU;
            transaction.CustomerId = updatedTransaction.CustomerId;
            transaction.DateOfTransaction = updatedTransaction.DateOfTransaction;

            //Save the changes
            await _context.SaveChangesAsync();
            return Ok("Your transaction was updated");
        }
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTransaction([FromUri]int id)
        {
           
            Transaction transaction = await _context.Transactions.FindAsync(id);
            if (transaction is null)
                return NotFound();

            _context.Transactions.Remove(transaction);
            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The restaurant was deletd.");
            }
            return InternalServerError();

        }
    }
}
