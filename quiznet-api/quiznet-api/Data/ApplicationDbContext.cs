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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Sender)
                .WithMany(p => p.FriendshipsOutgoing)
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Receiver)
                .WithMany(p => p.FriendshipsIncoming)
                .HasForeignKey(f => f.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(p => p.MessagesIncoming)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(p => p.MessagesOutgoing)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<BlockedPlayer>()
                .HasOne(p => p.BlockingPlayer)
                .WithMany(p => p.BlockedPlayers)
                .HasForeignKey(m => m.BlockingPlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BlockedPlayer>()
                .HasOne(p => p.PlayerBlocked)
                .WithMany(p => p.BlockedByPlayers)
                .HasForeignKey(m => m.PlayerBlockedId)
                .OnDelete(DeleteBehavior.NoAction);
            
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

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<Message> Messages { get; set; }
        
        public DbSet<BlockedPlayer> BlockedPlayers { get; set; }
    }
}
