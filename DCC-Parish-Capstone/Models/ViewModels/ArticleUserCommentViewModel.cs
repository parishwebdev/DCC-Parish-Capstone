using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models.ViewModels
{
    public class ArticleUserCommentViewModel
    {
        public Article Article { get; set; } 

        public Comment Comment { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}