using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DCC_Parish_Capstone.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "User Name")]
        public string Nickname { get; set; }

        public int Points { get; set; }

        [ForeignKey("Rank")]
        [Display(Name = "Rank")]
        public int RankId { get; set; }
        public Rank Rank { get; set; }

        public IEnumerable<UserBadge> EarnedBagdges { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.Language> Languages { get; set; }
        public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.BestPractice> BestPractices { get; set; }
        public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.Rank> Ranks { get; set; }
        public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.Badge> Badges { get; set; }


        public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.Article> Articles { get; set; }
        public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.Comment> Comments { get; set; }
        public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.CommentNotification> CommentNotifications { get; set; }
        public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.Subscription> Subscriptions { get; set; }
         public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.ArticleNotification> ArticleNotifications { get; set; }
         public System.Data.Entity.DbSet<DCC_Parish_Capstone.Models.UserBadge> UserBadges { get; set; }

    }
}