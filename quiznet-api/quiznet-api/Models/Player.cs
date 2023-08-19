using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models
{
    
    public class Player
    {
        public int Id { get; set; }
        
        public virtual User User { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [InverseProperty("Receiver")]
        public virtual ICollection<Friendship> FriendshipsIncoming { get; set; }

        [InverseProperty("Sender")]
        public virtual ICollection<Friendship> FriendshipsOutgoing { get; set; }

        [InverseProperty("Receiver")]
        public virtual ICollection<Message> MessagesIncoming { get; set; }

        [InverseProperty("Sender")]        
        public virtual ICollection<Message> MessagesOutgoing { get; set; }
        
        [InverseProperty("BlockingPlayer")]
        public virtual ICollection<BlockedPlayer> BlockedPlayers { get; set; }
        
        [InverseProperty("PlayerBlocked")]
        public virtual ICollection<BlockedPlayer> BlockedByPlayers { get; set; }

        public int Score { get; set; }

        public DateTime LastOnline { get; set; }

        public string? IpAddress { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        
        public int AvatarId { get; set; }
    }
}
