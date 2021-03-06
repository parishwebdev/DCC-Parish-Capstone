﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string AspNetUserId { get; set; }
        public ApplicationUser CommentAuthor { get; set; }

        [Display(Name = "Comment Text")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public int ParentId { get; set; }

        [ForeignKey("Article")]
        [Display(Name = "Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

    }
}