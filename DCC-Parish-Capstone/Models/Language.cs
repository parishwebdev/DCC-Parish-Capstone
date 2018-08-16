using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        //potienally highlight.js value(s)
    }
}