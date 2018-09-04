using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCC_Parish_Capstone.Models;

namespace DCC_Parish_Capstone.Controllers
{
    public class CommentNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CommentNotifications
        public ActionResult Index()
        {
            var commentNotifications = db.CommentNotifications.Include(c => c.Comment);
            return View(commentNotifications.ToList());
        }

  

        // POST: CommentNotifications/Delete/5  
        public ActionResult DeleteConfirmed(int id)
        {
            CommentNotification commentNotification = db.CommentNotifications.Find(id);
            db.CommentNotifications.Remove(commentNotification);
            db.SaveChanges();
            return RedirectToAction("UserPortal","Manage",null);
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
