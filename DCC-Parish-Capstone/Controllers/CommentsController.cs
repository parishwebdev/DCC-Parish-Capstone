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

                TriggerCommentNotification(comment);
                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
            }
             
            return RedirectToAction("Details", "Articles", new { id = ArticleId });
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
            db.SaveChanges();

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
            else if (loggedInUser.Points >= 150 && loggedInUser.Points >= 299)
            {
                loggedInUser.RankId = 3;
                db.SaveChanges();

            }
            else if (loggedInUser.Points >= 300)
            {
                loggedInUser.RankId = 4;
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
            db.Comments.Remove(comment);
            db.SaveChanges();

            UpdateMinusUserPoints(18, GetCurrentLoggedInUserId());

            return RedirectToAction("Details", "Articles", new { id = articleid });
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
            db.SaveChanges();

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
            else if (loggedInUser.Points >= 150 && loggedInUser.Points >= 299)
            {
                loggedInUser.RankId = 3;
                db.SaveChanges();

            }
            else if (loggedInUser.Points >= 300)
            {
                loggedInUser.RankId = 4;
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
