using DCC_Parish_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace DCC_Parish_Capstone.Helpers
{
    public class UserPortalHelper
    {

        public void SetCommentNotificationCommentsAuthorsArticles(IEnumerable<CommentNotification> commentNotifications, ApplicationDbContext db)
        {
            SetComments(commentNotifications, db);
            SetCommentAuthors(commentNotifications, db);
            SetCommentArticles(commentNotifications, db);
        }

        private void SetComments(IEnumerable<CommentNotification> commentNotifications, ApplicationDbContext db)
        {
            int i = 0;
            foreach (var item in commentNotifications)
            {
                Comment comment = db.Comments.Find(item.CommentId);

                commentNotifications.ElementAt(i).Comment = comment;
                i++;
            }
        }
        private void SetCommentAuthors(IEnumerable<CommentNotification> commentNotifications, ApplicationDbContext db)
        {
            int i = 0;
            foreach (var item in commentNotifications)
            {
                var commentUserId = item.Comment.AspNetUserId;
                commentNotifications.ElementAt(i).Comment.CommentAuthor = db.Users.Where(u => u.Id == commentUserId).Single();
                i++;
            }
        }
        private void SetCommentArticles(IEnumerable<CommentNotification> commentNotifications, ApplicationDbContext db)
        {
            int i = 0;
            foreach (var item in commentNotifications)
            {
                var commentUserId = item.Comment.AspNetUserId;
                var articleId = item.Comment.ArticleId;
                commentNotifications.ElementAt(i).Comment.Article = db.Articles.Where(a => a.Id == articleId).Single();
                i++;
            }
        }


        public void SetSubscriptionArticleNotification(IEnumerable<Subscription> subscriptions, ApplicationDbContext db)
        {

            foreach (var sub in subscriptions)
            {
                sub.SubscriptionArticleNotification = db.ArticleNotifications.Include(a => a.Article).Where(an => an.SubscriptionId == sub.Id).Where(an => an.AspNetUserId == sub.AspNetUserId);
            }
             
        }

    }
}