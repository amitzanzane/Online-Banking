using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankSystem.Models
{
    public class Statement
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime EndDate { get; set; }
        public List<Statemomentum> StatementList { get; set; }
    }
    public class Statemomentum
    {
        public string Name { get; set; }
        public DateTime TranDate { get; set; }
        public float Amount { get; set; }
        public string TranType { get; set; }
    }
}