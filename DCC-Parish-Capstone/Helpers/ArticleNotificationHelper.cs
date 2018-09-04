using DCC_Parish_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Helpers
{
    public class ArticleNotificationHelper
    {

        public void TriggerArticleNotification(Article article, ApplicationDbContext db)
        {

            foreach (var subscription in db.Subscriptions)
            {
                if (subscription.LanguageId == article.LanguageId && subscription.BestPracticeId == 12)
                {
                    CreateArticleNotificationForSubscription(article, subscription, db);
                }
                if (subscription.LanguageId == article.LanguageId && subscription.BestPracticeId == article.BestPracticeId)
                {
                    CreateArticleNotificationForSubscription(article, subscription, db);
                }

            }
            db.SaveChanges();

        }

        private void CreateArticleNotificationForSubscription(Article article, Subscription subscription, ApplicationDbContext db)
        {
            ArticleNotification articleNotification = new ArticleNotification();
            articleNotification.ArticleId = article.Id;

            articleNotification.AspNetUserId = subscription.AspNetUserId;
            articleNotification.SubscriptionId = subscription.Id;

            db.ArticleNotifications.Add(articleNotification);
        }

    }
}