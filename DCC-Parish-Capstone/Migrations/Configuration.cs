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


        }
    }
}
