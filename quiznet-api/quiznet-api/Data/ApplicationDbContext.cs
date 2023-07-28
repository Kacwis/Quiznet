using Microsoft.EntityFrameworkCore;
using quiznet_api.Model;
using quiznet_api.Models;

namespace quiznet_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        { 

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();            
        }

        

        public DbSet<WordTranslation> WordTranslations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Player> Players { get; set; }  

        public DbSet<Question> Questions { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<PlayerAnswer> PlayerAnswers { get; set; }

        public DbSet<GameRound> Rounds { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Category> Categories { get; set; }


    }
}
