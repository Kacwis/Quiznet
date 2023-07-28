using quiznet_api.Model;

namespace quiznet_api.Repository.IRepository
{
    public interface IWordRepository : IRepository<WordTranslation>
    {
        Task<WordTranslation> UpdateAsync(WordTranslation entity);

    }
}
