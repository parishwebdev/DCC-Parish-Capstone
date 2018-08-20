using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCC_Parish_Capstone.Models;
using DCC_Parish_Capstone.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace DCC_Parish_Capstone.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.BestPractice).Include(a => a.Language);
            return View(articles.ToList());
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ArticleUserCommentViewModel aUCVM = new ArticleUserCommentViewModel();
             
            aUCVM.Article = db.Articles.Include(a => a.BestPractice).Include(a => a.Language).Single(a => a.Id == id);
            var articleUserId = aUCVM.Article.AspNetUserId;
            aUCVM.ArticleAuthor = db.Users.Where(u => u.Id == articleUserId).Single();


            aUCVM.Comments = db.Comments.Where(c => c.ArticleId == aUCVM.Article.Id);
            SetCommentAuthors(aUCVM.Comments);


            if (aUCVM.Article == null)
            {
                return HttpNotFound();
            }

            return View(aUCVM);
        }

        private void SetCommentAuthors(IEnumerable<Comment> comments)
        {
            int i = 0;
            foreach (var item in comments)
            {
                var commentUserId = item.AspNetUserId;
                comments.ElementAt(i).CommentAuthor = db.Users.Where(u => u.Id == commentUserId).Single(); 
                i++;
            }
        }


        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.BestPracticeId = new SelectList(db.BestPractices, "Id", "Name");
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,FeaturedCode,LanguageId,BestPracticeId")] Article article)
        {
            if (ModelState.IsValid)
            {
                InitArticle(article);
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BestPracticeId = new SelectList(db.BestPractices, "Id", "Name", article.BestPracticeId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", article.LanguageId);
            return View(article);
        }

        private void InitArticle(Article article)
        {
            article.UpVotes = 0;
            article.DownVotes = 0;

            DateTime today = DateTime.Now;
            article.DateCreated = today;

            article.AspNetUserId = User.Identity.GetUserId();
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.BestPracticeId = new SelectList(db.BestPractices, "Id", "Name", article.BestPracticeId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", article.LanguageId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,FeaturedCode,LanguageId,BestPracticeId,UpVotes,DownVotes,DateCreated,AspNetUserId")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BestPracticeId = new SelectList(db.BestPractices, "Id", "Name", article.BestPracticeId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", article.LanguageId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
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
