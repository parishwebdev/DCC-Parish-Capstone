using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models
{
    public class ArticleNotification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Article")]
        [Display(Name = "Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        [ForeignKey("Subscription")]
        [Display(Name = "Subscription")]
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        public string AspNetUserId { get; set; }
    }
}