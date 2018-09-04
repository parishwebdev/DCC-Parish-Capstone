using DCC_Parish_Capstone.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Helpers
{
    public class CommentHelper
    {


        public void InitComment(Comment comment)
        {
            DateTime today = DateTime.Now;
            comment.DateCreated = today;

            comment.AspNetUserId = HttpContext.Current.User.Identity.GetUserId(); 
        }

    }
}