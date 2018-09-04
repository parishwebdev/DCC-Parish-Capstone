using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCC_Parish_Capstone.Helpers;
using DCC_Parish_Capstone.Models;
using Microsoft.AspNet.Identity;

namespace DCC_Parish_Capstone.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        private CommentHelper commentHelper = new CommentHelper();
        private UserGamifyHelper gamifyHelper = new UserGamifyHelper();
        private CommentNotificationHelper commentNotificationHelper = new CommentNotificationHelper();


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
                commentHelper.InitComment(comment);
                comment.ArticleId = ArticleId;
                comment.ParentId = (int)ParentId;
                 
                gamifyHelper.UpdateAddUserPoints(10, comment.AspNetUserId, db);

                db.Comments.Add(comment);
                db.SaveChanges();
                 
                gamifyHelper.EvaulateCommentBadgeEarned(comment.ArticleId, db);
                 
                commentNotificationHelper.TriggerCommentNotification(comment, db);

                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
            }
             
            return RedirectToAction("Details", "Articles", new { id = ArticleId });
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
             
            gamifyHelper.UpdateMinusUserPoints(5, User.Identity.GetUserId(),db);

            return RedirectToAction("Details", "Articles", new { id = articleid });
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
