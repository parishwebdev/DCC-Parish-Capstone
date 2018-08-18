using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Featured Code")]
        public string FeaturedCode { get; set; }

        [ForeignKey("Language")]
        [Display(Name = "Language")]
        public int LanguageId { get; set; }
        public Language Language { get; set; }

        [ForeignKey("BestPractice")]
        [Display(Name = "Best Practice")]
        public int BestPracticeId { get; set; }
        public BestPractice BestPractice { get; set; }

        /* ~~~ make seperate if time later v (both) ~~~ */
        [Display(Name = "Up Votes")]
        public int UpVotes { get; set; }

        [Display(Name = "Down Votes")]
        public int DownVotes { get; set; }
        /* ~~~ ^ (both) ~~~ */

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public string AspNetUserId { get; set; }

    }
}