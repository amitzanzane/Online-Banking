using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankSystem.Models
{
    public class login
    {
        //All the square bracket functions or DataAnnotations can be used for Validations on Form submit
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Date Of Birth :")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime DOB { get; set; }
        [Required]
        [StringLength(12)]
        public string Phone { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 7)]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string username { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string password { get; set; }
        [Required]
        public string Branch { get; set; }
        public List<Branches> Branchs { get; set; }
        [Required]
        public string AccountType { get; set; }
    }

    public class Branches
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string IFSC { get; set; }
        [Required]
        public string Contact { get; set; }
    }
}