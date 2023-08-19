using quiznet_api.Data;
using quiznet_api.Models;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Repository;

public class BlockedPlayerRepository : Repository<BlockedPlayer>, IBlockedPlayerRepository
{
    public BlockedPlayerRepository(ApplicationDbContext db) : base(db)
    {
    }
    
    
}