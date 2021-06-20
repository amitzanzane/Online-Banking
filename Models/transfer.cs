using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankSystem.Models
{
    public class transfer
    {
        public string AccountNumber { get; set; }
        public string IFSC { get; set; }
        public string AccountHolder { get; set; }
        public float Amount { get; set; }
    }
}