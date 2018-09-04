using DCC_Parish_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Helpers
{
    public class UserGamifyHelper
    { 

        private ApplicationUser InitUserFromUserId(string userId, ApplicationDbContext db)
        { 
            var loggedInUser = db.Users.Include(u => u.Rank).Where(u => u.Id == userId).Single();
            return loggedInUser;
        }

        private void EvaluateRank(ApplicationUser loggedInUser, ApplicationDbContext db)
        { 
            if (loggedInUser.Points >= 0 && loggedInUser.Points <= 49)
            {
                loggedInUser.RankId = 1;
                db.SaveChanges();
            }
            else if (loggedInUser.Points >= 50 && loggedInUser.Points <= 149)
            {
                loggedInUser.RankId = 2;
                db.SaveChanges();
            }
            else if (loggedInUser.Points >= 150 && loggedInUser.Points <= 299)
            {
                loggedInUser.RankId = 3;
                db.SaveChanges(); 
            }
            else if (loggedInUser.Points >= 300)
            {
                loggedInUser.RankId = 4;
                db.SaveChanges(); 
            }
            else
            { 
                db.SaveChanges();
            }
        }

        public void UpdateAddUserPoints(int numPtsToAdd, string userIdToAddPtsTo, ApplicationDbContext db)
        {
            var loggedInUser = InitUserFromUserId(userIdToAddPtsTo,db); 
            loggedInUser.Points += numPtsToAdd; 
            EvaluateRank(loggedInUser, db); 
        }


        public void UpdateMinusUserPoints(int numPtsToMinus, string userIdToAddPtsTo, ApplicationDbContext db)
        { 
            var loggedInUser = InitUserFromUserId(userIdToAddPtsTo, db); 
            loggedInUser.Points -= numPtsToMinus;
            EvaluateRank(loggedInUser, db);

        }

        public void EvaulateCommentBadgeEarned(int articleId, ApplicationDbContext db)
        {

            Article article = GetCommentArticle(articleId, db);
            int commentCount = GetCommentNumberOfFromArticle(article, db);

            if (commentCount >= 5 && commentCount <= 9)
            {
                AssignBadgeToUser(article.AspNetUserId, 4, 5, db);
            }
            else if (commentCount >= 10 && commentCount <= 24)
            {
                AssignBadgeToUser(article.AspNetUserId, 5, 10, db);
            }
            else if (commentCount >= 25)
            {
                AssignBadgeToUser(article.AspNetUserId, 6, 15, db);
            }

        }


        public void EvaluateUpvoteBadgeEarned(Article article, ApplicationDbContext db)
        {
            if (article.UpVotes == 10)
            { 
                CreateBadge(article.AspNetUserId, 1, db); 
                UpdateAddUserPoints(5, article.AspNetUserId, db);
            }
            else if (article.UpVotes == 25)
            {  
                CreateBadge(article.AspNetUserId, 2, db);
                UpdateAddUserPoints(10, article.AspNetUserId, db);
            }
            else if (article.UpVotes == 50)
            {  
                CreateBadge(article.AspNetUserId, 3, db);
                UpdateAddUserPoints(25, article.AspNetUserId, db);
            }
        }



        private void AssignBadgeToUser(string userId, int badgeid, int potienallyPtsEarned, ApplicationDbContext db)
        {

            var badgesByUserId = db.UserBadges.Where(ub => ub.AspNetUserId == userId);

            if (badgesByUserId.Count() > 0)
            { 
                for (int i = 0; i < badgesByUserId.Count(); i++)
                {
                    if (badgesByUserId.All(ub => ub.BadgeId != badgeid))
                    {
                        CreateBadge(userId, badgeid, db); 
                        UpdateAddUserPoints(potienallyPtsEarned, userId, db);
                        break;
                    }
                }

            }
            else
            {
                CreateBadge(userId,badgeid,db); 
                UpdateAddUserPoints(potienallyPtsEarned, userId, db);
            }

        }

        private void CreateBadge(string userId, int badgeid, ApplicationDbContext db)
        {
            UserBadge userBadge = new UserBadge();
            userBadge.AspNetUserId = userId;
            userBadge.BadgeId = badgeid;

            db.UserBadges.Add(userBadge);
            db.SaveChanges();
        }
         
        private Article GetCommentArticle(int articleid, ApplicationDbContext db)
        {
            Article article = db.Articles.Find(articleid);
            return article;
        }
        private int GetCommentNumberOfFromArticle(Article article, ApplicationDbContext db)
        {
            int commentCount = db.Comments.Where(c => c.ArticleId == article.Id).Count();
            return commentCount;
        }


    }
}