using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models
{
    public class CommentNotification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Comment")]
        [Display(Name = "Comment")]
        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public string AspNetUserId { get; set; }
    }
}