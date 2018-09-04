using DCC_Parish_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Helpers
{
    public class CommentNotificationHelper
    {

        public void TriggerCommentNotification(Comment comment, ApplicationDbContext db)
        {
            CommentNotification commentNotification = new CommentNotification();
            commentNotification.CommentId = comment.Id;
            commentNotification.AspNetUserId = GetCommentArticleAuthorId(comment.ArticleId,db);

            db.CommentNotifications.Add(commentNotification);
            db.SaveChanges();
        }

        private string GetCommentArticleAuthorId(int articleid, ApplicationDbContext db)
        {
            Article article = db.Articles.Find(articleid);
            return article.AspNetUserId;
        }

    }
}