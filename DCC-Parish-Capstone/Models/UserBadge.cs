using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DCC_Parish_Capstone.Models
{
    public class UserBadge
    {

        [Key]
        public int Id { get; set; }

        public string AspNetUserId { get; set; }

        [ForeignKey("Badge")]
        [Display(Name = "Badge")]
        public int BadgeId { get; set; }
        public Badge Badge { get; set; }

    }
}