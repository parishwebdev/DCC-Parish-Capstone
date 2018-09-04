using DCC_Parish_Capstone.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Helpers
{
    public class ArticleHelper
    {


        public void InitArticle(Article article)
        {
            article.UpVotes = 0;
            article.DownVotes = 0;

            DateTime today = DateTime.Now;
            article.DateCreated = today;

            article.AspNetUserId = HttpContext.Current.User.Identity.GetUserId();
        }


        public void SetArticleAuthors(IEnumerable<Article> articles, ApplicationDbContext db)
        {
            int i = 0;
            foreach (var item in articles)
            {
                var articleUserId = item.AspNetUserId;
                articles.ElementAt(i).ArticleAuthor = db.Users.Include(u => u.Rank).Where(u => u.Id == articleUserId).Single();
                articles.ElementAt(i).ArticleAuthor.EarnedBagdges = db.UserBadges.Include(ub => ub.Badge).Where(ub => ub.AspNetUserId == articleUserId);

                i++;
            }
        }
        public void SetArticleCommentAuthors(IEnumerable<Comment> comments, ApplicationDbContext db)
        {
            int i = 0;
            foreach (var item in comments)
            {
                var commentUserId = item.AspNetUserId;
                comments.ElementAt(i).CommentAuthor = db.Users.Include(u => u.Rank).Where(u => u.Id == commentUserId).Single();
                comments.ElementAt(i).CommentAuthor.EarnedBagdges = db.UserBadges.Include(ub => ub.Badge).Where(ub => ub.AspNetUserId == commentUserId);
                i++;
            }
        }


    }
}