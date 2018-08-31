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

namespace DCC_Parish_Capstone.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index()
        {

           var Articles = db.Articles.Include(a => a.BestPractice).Include(a => a.Language);
           SetArticleAuthors(Articles);

            return View(Articles);
        }

        private void SetArticleAuthors(IEnumerable<Article> articles)
        {
            int i = 0;
            foreach (var item in articles)
            {
                var articleUserId = item.AspNetUserId;
                articles.ElementAt(i).ArticleAuthor = db.Users.Include(u => u.Rank).Where(u => u.Id == articleUserId).Single();
                articles.ElementAt(i).ArticleAuthor.EarnedBagdges =  db.UserBadges.Include(ub => ub.Badge).Where(ub => ub.AspNetUserId == articleUserId);

                i++;
            }
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
            aUCVM.Article.ArticleAuthor = db.Users.Include(u => u.Rank).Where(u => u.Id == articleUserId).Single();


            aUCVM.Comments = db.Comments.Where(c => c.ArticleId == aUCVM.Article.Id);
            SetCommentAuthors(aUCVM.Comments);

            aUCVM.Article.ArticleAuthor.EarnedBagdges = db.UserBadges.Include(ub => ub.Badge).Where(ub => ub.AspNetUserId == articleUserId);

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
                comments.ElementAt(i).CommentAuthor = db.Users.Include(u => u.Rank).Where(u => u.Id == commentUserId).Single();
                comments.ElementAt(i).CommentAuthor.EarnedBagdges = db.UserBadges.Include(ub => ub.Badge).Where(ub => ub.AspNetUserId == commentUserId);
                i++;
            }
        }


        //HERE
        public ActionResult UpvoteArticle(int articleId)
        {
            
            Article article = db.Articles.Find(articleId);
            article.UpVotes += 1;
            db.SaveChanges();

            var userId = GetCurrentLoggedInUserId();
            UpdateUserPoints(5, userId);

            EvaluateUpvoteBadgeEarned(article);

            return RedirectToAction("Details", new { id = article.Id });
        }

        //HERE
        public ActionResult DownvoteArticle(int articleId)
        {
            Article article = db.Articles.Find(articleId);
            article.DownVotes += 1;
            db.SaveChanges();

            var userId = GetCurrentLoggedInUserId();
            UpdateUserPoints(5, userId);

            return RedirectToAction("Details", new { id = article.Id });
        }

        private void EvaluateUpvoteBadgeEarned(Article article)
        {
            if (article.UpVotes == 10)
            {
                AssignBadgeToUser(article.AspNetUserId, 1);
                UpdateUserPoints(5, article.AspNetUserId);
            }
            else if (article.UpVotes == 25)
            {
                AssignBadgeToUser(article.AspNetUserId, 2);
                UpdateUserPoints(10, article.AspNetUserId);
            }
            else if (article.UpVotes == 50)
            {
                AssignBadgeToUser(article.AspNetUserId, 2);
                UpdateUserPoints(25, article.AspNetUserId);
            }
        }

        private void AssignBadgeToUser(string userId, int badgeid)
        {
            UserBadge upvoteBadge = new UserBadge();
            upvoteBadge.AspNetUserId = userId;
            upvoteBadge.BadgeId = badgeid;
            db.UserBadges.Add(upvoteBadge);
            db.SaveChanges();
        }

        //here  (look to see if implented anywhere else)
        private String GetCurrentLoggedInUserId()
        {
            var userId = User.Identity.GetUserId(); 
            return userId;
        }

        public ActionResult TopArticles()
        {
            var topArticles = db.Articles.Include(a => a.BestPractice).Include(a => a.Language).OrderByDescending(a => a.UpVotes).Take(3);

            topArticles.OrderBy(a => a.DownVotes);

            SetArticleAuthors(topArticles);

            return View(topArticles);
        }

        //HERE
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
                InitArticle(article);
                db.Articles.Add(article);
                db.SaveChanges();
                TriggerArticleNotification(article);

                return RedirectToAction("Index");
            }

            ViewBag.BestPracticeId = new SelectList(db.BestPractices.Where(bp => bp.Id != 12), "Id", "Name", article.BestPracticeId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", article.LanguageId);
            return View(article);
        }

        //HERE
        private void TriggerArticleNotification(Article article)
        {

            foreach (var subscription in db.Subscriptions)
            {
                if (subscription.LanguageId == article.LanguageId && subscription.BestPracticeId == 12)
                {
                    CreateArticleNotificationForSubscription(article, subscription);
                }
                if (subscription.LanguageId == article.LanguageId && subscription.BestPracticeId == article.BestPracticeId)
                {
                    CreateArticleNotificationForSubscription(article, subscription);
                }
                 
            }
            db.SaveChanges();

        }

        //HERE ? 
        private void CreateArticleNotificationForSubscription(Article article, Subscription subscription)
        {
            ArticleNotification articleNotification = new ArticleNotification();
            articleNotification.ArticleId = article.Id; 

            articleNotification.AspNetUserId = subscription.AspNetUserId;
            articleNotification.SubscriptionId = subscription.Id;

            db.ArticleNotifications.Add(articleNotification);
        }

        //HERE ?
        private void InitArticle(Article article)
        {
            article.UpVotes = 0;
            article.DownVotes = 0;

            DateTime today = DateTime.Now;
            article.DateCreated = today;

            article.AspNetUserId = User.Identity.GetUserId();

            //Points - Call addpts method
            UpdateUserPoints(35, article.AspNetUserId);
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

            UpdateMinusUserPoints(18, GetCurrentLoggedInUserId());

            return RedirectToAction("Index");
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

        //HERE ? 
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitAction(string html,string articleName)
        {
            // read parameters from the webpage
            string htmlString = html;
              
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.CssMediaType = HtmlToPdfCssMediaType.Screen; 
            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlString);
            

            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = articleName +".pdf";
            return fileResult;

        }

        //HERE ?  
        [ValidateInput(false)]
        public ActionResult SendEmail(string to, string html, string articleTitle, int articleId)
        {

            MailMessage m = new MailMessage();
            m.Subject = articleTitle + " article from Best Practice Wiki";
            m.Body =  html;
            m.IsBodyHtml = true;

            m.From = new MailAddress("bestpracticeswiki@gmail.com");

            m.To.Add(new MailAddress(to));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";


            var userName = System.Web.Configuration.WebConfigurationManager.AppSettings["gEmailUsername"];
            var password = System.Web.Configuration.WebConfigurationManager.AppSettings["gPassword"];

            NetworkCredential authinfo = new NetworkCredential(userName, password);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = authinfo;
            smtp.EnableSsl = true;
            smtp.Send(m);


            //keep but extract to new helper ^
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
