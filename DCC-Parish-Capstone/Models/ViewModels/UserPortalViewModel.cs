using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models.ViewModels
{
    public class UserPortalViewModel
    {
        public ApplicationUser CurrentWebDev { get; set; }

        public IEnumerable<CommentNotification> commentNotifications { get; set; }

        public IEnumerable<Subscription> subscriptions { get; set; }
    }
}