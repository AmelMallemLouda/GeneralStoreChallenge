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
    public class CustomerController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        [HttpPost]
        public async Task<IHttpActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (customer is null)
            {
                return BadRequest("the Customer does not exist");
            }

            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return Ok("The customer was successfully created");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetAllCustomers()
        {
            List<Customer> customer = await _context.Customers.ToListAsync();
            return Ok(customer);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetCustomerById([FromUri] int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);
            if(customer is null)
            {
                return NotFound();
            }
            return Ok(customer);

        }
        [HttpPut]

        public async Task<IHttpActionResult> UpdateCustomer([FromUri] int id, [FromBody] Customer updatedCustomer)
        {
            //Chech the Ids if they match
            if (id != updatedCustomer?.Id)
            {
                return BadRequest("Ids do not match");
            }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                    
                //Find the restaurant in the database
                Customer customer = await _context.Customers.FindAsync(id);
                //if the restaurant does not exist then do something 
                if (customer is null)
                    return NotFound();
                //Update the Propreties 
                customer.FirstName = updatedCustomer.FirstName;
                customer.LastName = updatedCustomer.LastName;
                //Save the changes
                await _context.SaveChangesAsync();
                return Ok("The customer was updated");

            
        }
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCustomer([FromUri] int Id)
        {
            //Find the customer
            Customer customer = await _context.Customers.FindAsync(Id);
            if (customer is null)
                return NotFound();
            _context.Customers.Remove(customer);
            if(await _context.SaveChangesAsync()==1)
            {
                return Ok("The customer was succefully deleted");
            }
            return InternalServerError();
                
        }

    }
}
