using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DCC_Parish_Capstone.Models;
using DCC_Parish_Capstone.Models.ViewModels;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNet.Identity;
using SelectPdf;
using System.IO;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using static Google.Apis.Gmail.v1.GmailService;
using Google.Apis.Services;
using System.Text;
using MimeKit;
using DCC_Parish_Capstone.Helpers;

namespace DCC_Parish_Capstone.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Helpers
        ArticleHelper articleHelper = new ArticleHelper();
        UserGamifyHelper gamifyHelper = new UserGamifyHelper();
        ArticleNotificationHelper articleNotificationHelper = new ArticleNotificationHelper();
        PdfHelper pdfHelper = new PdfHelper();
        EmailHelper emailHelper = new EmailHelper();


        // GET: Articles
        public ActionResult Index()
        {

           var Articles = db.Articles.Include(a => a.BestPractice).Include(a => a.Language);
       
            articleHelper.SetArticleAuthors(Articles, db);

            return View(Articles);
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
            aUCVM.Article.ArticleAuthor = db.Users.Include(u => u.Rank).Where(u => u.Id == aUCVM.Article.AspNetUserId).Single();
             
            aUCVM.Comments = db.Comments.Where(c => c.ArticleId == aUCVM.Article.Id);
         
            articleHelper.SetArticleCommentAuthors(aUCVM.Comments, db);

            aUCVM.Article.ArticleAuthor.EarnedBagdges = db.UserBadges.Include(ub => ub.Badge).Where(ub => ub.AspNetUserId == aUCVM.Article.AspNetUserId);

            if (aUCVM.Article == null)
            {
                return HttpNotFound();
            }

            return View(aUCVM);
        }

  

        //HERE
        public ActionResult UpvoteArticle(int articleId)
        {
            
            Article article = db.Articles.Find(articleId);
            article.UpVotes += 1;
            db.SaveChanges();
              
            gamifyHelper.UpdateAddUserPoints(5, User.Identity.GetUserId(), db);
             
            gamifyHelper.EvaluateUpvoteBadgeEarned(article, db);

            return RedirectToAction("Details", new { id = article.Id });
        }

        //HERE
        public ActionResult DownvoteArticle(int articleId)
        {
            Article article = db.Articles.Find(articleId);
            article.DownVotes += 1;
            db.SaveChanges();
              
            gamifyHelper.UpdateAddUserPoints(5, User.Identity.GetUserId(), db);

            return RedirectToAction("Details", new { id = article.Id });
        }
 

        public ActionResult TopArticles()
        {
            var topArticles = db.Articles.Include(a => a.BestPractice).Include(a => a.Language).OrderByDescending(a => a.UpVotes).Take(3);

            topArticles.OrderBy(a => a.DownVotes);
                        
            articleHelper.SetArticleAuthors(topArticles, db);

            return View(topArticles);
        }
          
        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.BestPracticeId = new SelectList(db.BestPractices.Where(bp => bp.Id != 12), "Id", "Name");
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
                articleHelper.InitArticle(article); 
                gamifyHelper.UpdateAddUserPoints(35, article.AspNetUserId, db);

                db.Articles.Add(article);
                db.SaveChanges();
               
                articleNotificationHelper.TriggerArticleNotification(article, db);

                return RedirectToAction("Index");
            }

            ViewBag.BestPracticeId = new SelectList(db.BestPractices.Where(bp => bp.Id != 12), "Id", "Name", article.BestPracticeId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", article.LanguageId);
            return View(article);
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
            ViewBag.BestPracticeId = new SelectList(db.BestPractices.Where(bp => bp.Id != 12), "Id", "Name", article.BestPracticeId);
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
            ViewBag.BestPracticeId = new SelectList(db.BestPractices.Where(bp => bp.Id != 12), "Id", "Name", article.BestPracticeId);
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
             
            gamifyHelper.UpdateMinusUserPoints(18, User.Identity.GetUserId(), db);

            return RedirectToAction("Index");
        }
         
        public ActionResult ArticlesByLanguageandBestPractice(int LangId, int BestPractId)
        {

            var articlesByLanguageandBestPractice = db.Articles.Where(a => a.LanguageId == LangId).Where(a => a.BestPracticeId == BestPractId).Include(a => a.BestPractice).Include(a => a.Language);
             
            return View(articlesByLanguageandBestPractice);

        }

        public ActionResult ArticlesByLanguage(int LangId)
        {

            var articlesByLanguage = db.Articles.Where(a => a.LanguageId == LangId).Include(a => a.BestPractice).Include(a => a.Language);

            return View(articlesByLanguage);

        }
         
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitAction(string html,string articleName)
        { 
            var fileResult = pdfHelper.DownloadArticleAsPDF(html, articleName); 
            return fileResult;
        }
         
        [ValidateInput(false)]
        public ActionResult SendEmail(string to, string html, string articleTitle, int articleId)
        {
            emailHelper.EmailArticle(to, html, articleTitle, articleId); 
            return RedirectToAction("Details", new { id = articleId });
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
