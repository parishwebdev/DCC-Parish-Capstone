using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Language")]
        [Display(Name = "Language")]
        public int LanguageId { get; set; }
        public Language Language { get; set; }

        [ForeignKey("BestPracticeSub")]
        [Display(Name = "Best Practice")]
        public int BestPracticeId { get; set; }
        public BestPractice BestPracticeSub { get; set; }

        public string AspNetUserId { get; set; }


        public IEnumerable<ArticleNotification> SubscriptionArticleNotification { get; set; }

    }
}