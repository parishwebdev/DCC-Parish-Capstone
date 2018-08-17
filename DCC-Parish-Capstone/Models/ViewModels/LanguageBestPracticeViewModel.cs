using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models.ViewModels
{
    public class LanguageBestPracticeViewModel  
    {  
        [Display(Name = "Languages")]
        public IEnumerable<Language> Languages { get; set; }
        public IEnumerable<BestPractice> BestPractices { get; set; }


    }
}