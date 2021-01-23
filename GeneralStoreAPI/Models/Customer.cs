using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI.Models
{
    public class Customer
    {   [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
      
        //{get{ return $"{FirstName},{LastName}";}
      
        //Expression bodied auto proprety
        public string FullName => $"{FirstName} {LastName}";

       

    }
}