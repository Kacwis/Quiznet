using quiznet_api.Data;
using quiznet_api.Model;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Repository
{
    public class WordRepository : Repository<WordTranslation>, IWordRepository
    {
        private readonly ApplicationDbContext _db;

        public WordRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<WordTranslation> UpdateAsync(WordTranslation entity)
        {
            _db.WordTranslations.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
