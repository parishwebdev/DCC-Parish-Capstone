using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models.ViewModels
{
    public class ArticleUserCommentViewModel
    {
        public Article Article { get; set; }
        public ApplicationUser ArticleAuthor { get; set; }

        //Comments Later
    }
}