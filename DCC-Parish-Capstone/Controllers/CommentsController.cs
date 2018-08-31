using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCC_Parish_Capstone.Models;
using Microsoft.AspNet.Identity;

namespace DCC_Parish_Capstone.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Article);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int ArticleId, int  ParentId, [Bind(Include = "Id,AspNetUserId,Body")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                InitComment(comment);
                comment.ArticleId = ArticleId;
                comment.ParentId = (int)ParentId;

                UpdateUserPoints(10, comment.AspNetUserId);

                db.Comments.Add(comment);
                db.SaveChanges();

                EvaulateCommentBadgeEarned(comment.ArticleId);

                TriggerCommentNotification(comment);
                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
            }
             
            return RedirectToAction("Details", "Articles", new { id = ArticleId });
        }


        private void EvaulateCommentBadgeEarned(int articleId)
        {

            Article article = GetCommentArticle(articleId);
            int commentCount = GetCommentNumberOfFromArticle(article);

            if (commentCount >= 5 && commentCount <= 9)
            {
                AssignBadgeToUser(article.AspNetUserId, 4 , 5);
            }
            else if (commentCount >= 10 && commentCount <= 24)
            {
                AssignBadgeToUser(article.AspNetUserId, 5, 10);
            }
            else if (commentCount >= 25)
            {
                AssignBadgeToUser(article.AspNetUserId, 6, 15);
            }

        }

        private void AssignBadgeToUser(string userId, int badgeid, int potienallyPtsEarned)
        {
            
            var badgesByUserId = db.UserBadges.Where(ub => ub.AspNetUserId == userId);

            if (badgesByUserId.Count() > 0) {
                //foreach (var item in badgesByUserId)
                for (int i = 0; i < badgesByUserId.Count(); i++)
                {
                    if (badgesByUserId.All(ub => ub.BadgeId != badgeid))
                    {
                        UserBadge commentBadge = new UserBadge();
                        commentBadge.AspNetUserId = userId;
                        commentBadge.BadgeId = badgeid;

                        

                        db.UserBadges.Add(commentBadge);
                        db.SaveChanges();
                        UpdateUserPoints(potienallyPtsEarned, userId);
                        break;
                    }
                }
               
            } 

        }

        //HERE ? 
        private void TriggerCommentNotification(Comment comment)
        {
            CommentNotification commentNotification = new CommentNotification();
            commentNotification.CommentId = comment.Id;
            commentNotification.AspNetUserId = GetCommentArticleAuthorId(comment.ArticleId);

            db.CommentNotifications.Add(commentNotification);
            db.SaveChanges();
        }

        private string GetCommentArticleAuthorId(int articleid)
        {
            Article article = db.Articles.Find(articleid);
            return article.AspNetUserId;
        }
        private Article GetCommentArticle(int articleid)
        {
            Article article = db.Articles.Find(articleid);
            return article;
        }
        private int GetCommentNumberOfFromArticle(Article article)
        {
            int commentCount = db.Comments.Where(c => c.ArticleId == article.Id).Count();
            return commentCount;
        }

        //HERE ?  
        private void InitComment(Comment comment)
        {
            DateTime today = DateTime.Now;
            comment.DateCreated = today;
            
            comment.AspNetUserId = User.Identity.GetUserId();
        }


        //Here (helper class)
        public void UpdateUserPoints(int numPtsToAdd, string userIdToAddPtsTo)
        {
            //Extract to InitUserObject
            var userId = userIdToAddPtsTo;
            var loggedInUser = db.Users.Include(u => u.Rank).Where(u => u.Id == userId).Single();

            loggedInUser.Points += numPtsToAdd;

            //Extract to EvaluateRank
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
         


        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", comment.ArticleId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AspNetUserId,Body,DateCreated,ParentId,ArticleId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", comment.ArticleId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirmed(int id, int articleid)
        {
            Comment comment = db.Comments.Find(id);
                         
            EvaulateCommentBadgeUnEarned(comment.ArticleId);

            db.Comments.Remove(comment);
            db.SaveChanges();

            UpdateMinusUserPoints(5, GetCurrentLoggedInUserId());

            return RedirectToAction("Details", "Articles", new { id = articleid });
        }

        private void EvaulateCommentBadgeUnEarned(int articleId)
        {

            Article article = GetCommentArticle(articleId);
            int commentCount = GetCommentNumberOfFromArticle(article);

            if (commentCount == 5)
            {
                UnAssignBadgeToUser(article.AspNetUserId, 4);
            }
            else if (commentCount == 10)
            {
                UnAssignBadgeToUser(article.AspNetUserId, 5);
            }
            else if (commentCount == 25)
            {
                UnAssignBadgeToUser(article.AspNetUserId, 6);
            }

        }
        private void UnAssignBadgeToUser(string userId, int badgeid)
        {
             
            var badgesByUserId = db.UserBadges.Where(ub => ub.AspNetUserId == userId);

            if (badgesByUserId.Count() > 0)
            {
                foreach (var item in badgesByUserId)
                {
                    if (badgesByUserId.Any(ub => ub.BadgeId == badgeid))
                    {
                        UserBadge userBadge = db.UserBadges.Where(ub => ub.AspNetUserId == userId).Where(ub => ub.BadgeId == badgeid).Single();
                        db.UserBadges.Remove(userBadge);

                         
                        break;
                    }
                }
                UpdateMinusUserPoints(10, userId);
                db.SaveChanges();
            }


        }


        private String GetCurrentLoggedInUserId()
        {
            var userId = User.Identity.GetUserId();
            return userId;
        }



        public void UpdateMinusUserPoints(int numPtsToMinus, string userIdToAddPtsTo)
        {
            //Extract to InitUserObject
            var userId = userIdToAddPtsTo;
            var loggedInUser = db.Users.Include(u => u.Rank).Where(u => u.Id == userId).Single();

            loggedInUser.Points -= numPtsToMinus; 

            //Extract to EvaluateRank
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
