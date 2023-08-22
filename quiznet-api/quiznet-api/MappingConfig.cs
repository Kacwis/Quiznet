using AutoMapper;
using quiznet_api.Models;
using quiznet_api.Models.DTO;

namespace quiznet_api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            
            CreateMap<User, LogInUserResponseDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Answer, CreateAnswerDTO>().ReverseMap();
            CreateMap<Player, GameResponsePlayerDTO>().ReverseMap();
            CreateMap<Player, PlayerAnswerPlayerResponseDTO>().ReverseMap();
            CreateMap<Player, PlayerResponseDTO>().ReverseMap();
            CreateMap<GameRound, GameRoundResponseDTO>()
                .ForMember(x => x.PlayerAnswers, opt => opt.MapFrom(x => x.PlayerAnswers));
            CreateMap<Game, GameResponseDTO>()
                .ForMember(x => x.Players, opt => opt.MapFrom(x => x.Players))
                .ForMember(x => x.Rounds, opt => opt.MapFrom(x => x.Rounds));            
            CreateMap<PlayerAnswer, CreatePlayerAnswerDTO>().ReverseMap();
            CreateMap<PlayerAnswer, PlayerAnswerResponseDTO>()
                .ForMember(x => x.Player, opt => opt.MapFrom(x => x.Player))
                .ForMember(x => x.IsCorrect, opt => opt.MapFrom(src => IsCorrect(src)));
            CreateMap<GameRound, CreateGameRoundDTO>()
                .ForMember(x => x.PlayerAnswers, opt => opt.MapFrom(x => x.PlayerAnswers));
            
               
        }

        private bool IsCorrect(PlayerAnswer playerAnswer)
        {
            var allAnswers = playerAnswer.Question.Answers;
            var correctAnswer = allAnswers.FirstOrDefault(a => a.Id == playerAnswer.Question.CorrectAnswerId);
            return playerAnswer.SelectedAnswer == correctAnswer.Text;
        }
    }
}
