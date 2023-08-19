using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models;

public class BlockedPlayer
{
    public int Id { get; set; }
    
    public int PlayerBlockedId { get; set; }
    
    public int BlockingPlayerId { get; set; }
    
    public virtual Player PlayerBlocked { get; set; }
    
    public virtual Player BlockingPlayer { get; set; }
    
    public DateTime BlockedAt { get; set; }
}