using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Subscription")]
        [Display(Name = "Subscription")]
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        [ForeignKey("CommentNotification")]
        [Display(Name = "Comment Notification")]
        public int CommentNotificationId { get; set; }
        public CommentNotification CommentNotification { get; set; }

        public string AspNetUserId { get; set; }
    }
}