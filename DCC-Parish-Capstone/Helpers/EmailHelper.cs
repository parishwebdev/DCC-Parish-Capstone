using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace DCC_Parish_Capstone.Helpers
{
    public class EmailHelper
    {


        public void EmailArticle(string to, string html, string articleTitle, int articleId)
        {
            MailMessage m = new MailMessage();
            m.Subject = articleTitle + " article from Best Practice Wiki";
            m.Body = html;
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
        }


    }
}