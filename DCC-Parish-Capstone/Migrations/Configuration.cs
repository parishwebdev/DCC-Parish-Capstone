namespace DCC_Parish_Capstone.Migrations
{
    using DCC_Parish_Capstone.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DCC_Parish_Capstone.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DCC_Parish_Capstone.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //Rank
            context.Ranks.AddOrUpdate(
              r => r.Name,
              new Rank { Id = 1, Name = "Beginner" },
              new Rank { Id = 2, Name = "Average" },
              new Rank { Id = 3, Name = "Intermediate" },
              new Rank { Id = 4, Name = "Expect" }
            );

            //Languages
            context.Languages.AddOrUpdate(
             l => l.Name,
             new Language { Id = 1, Name = "C#" },
             new Language { Id = 2, Name = "Java" },
             new Language { Id = 3, Name = "Php" },
             new Language { Id = 4, Name = "Python" }
           );
            //Best Practices
            context.BestPractices.AddOrUpdate(
             bp => bp.Name,
            
             new BestPractice { Id = 1, Name = "Single Responsibility Principle" },
             new BestPractice { Id = 2, Name = "Open/Closed Principle" },
             new BestPractice { Id = 3, Name = "Liskov Substitution Principle" },
             new BestPractice { Id = 4, Name = "Interface Segregation Principle" },
             new BestPractice { Id = 5, Name = "Dependency Inversion Principle" },
             new BestPractice { Id = 6, Name = "Builder Pattern" },
             new BestPractice { Id = 7, Name = "Singleton Pattern" },
             new BestPractice { Id = 8, Name = "Adapter Pattern" },
             new BestPractice { Id = 9, Name = "Decorator Pattern" },
             new BestPractice { Id = 10, Name = "Strategy Pattern" },
             new BestPractice { Id = 11, Name = "Template Method Pattern" },
              new BestPractice { Id = 12, Name = "No Best Practice" }
           );



            //Badges
            context.Badges.AddOrUpdate(
             l => l.Name,
             new Badge { Id = 1, Name = "On of your Articles was Upvoted 10 times, your writing is gaining some popularity" },
             new Badge { Id = 2, Name = "On of your Articles was Upvoted 25 times, your writing is becoming a favorite" },
             new Badge { Id = 3, Name = "On of your Articles was Upvoted 50 times, your writing is up the there amoung the greats" },
             new Badge { Id = 4, Name = "On of your Articles was Commented on 5 times, people are starting to talk about your article" },
             new Badge { Id = 5, Name = "On of your Articles was Commented on 10 times, your article is gain some discussion" },
             new Badge { Id = 6, Name = "On of your Articles was Commented on 25 times, people seem to be talking a lot about your article" }
           );

        }
    }
}
