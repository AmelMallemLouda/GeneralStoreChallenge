using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI.Models
{
    public class Product
    {
        [Key]
        public string SKU { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }
        [Required]
        public int NumberInventory { get; set; }
        public  List<Product> Products { get; set; } = new List<Product>();
        public bool IsInStock
        {
            get
            {
               if(Products != null)
                {
                    return true;
                }
                return false;
            }
        }
        public int TotalProduct
        {
           get
            {
              return  Products.Count();
            }
        }
    }
}