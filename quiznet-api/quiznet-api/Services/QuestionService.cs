using AutoMapper;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;

namespace quiznet_api.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        private readonly IAnswerRepository _answerRepository;

        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapper _mapper;

        public QuestionService(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }


        public async Task<Question> CreateQuestion(CreateQuestionDTO createQuestionDTO)
        {
            var answers = _mapper.Map<ICollection<CreateAnswerDTO>, ICollection<Answer>>(createQuestionDTO.Answers);
            var createdAnswers = new List<Answer>();
            foreach (var answer in answers)
            {
                createdAnswers.Add(await _answerRepository.CreateAsync(answer));
            }
            var question = new Question()
            {
                Answers = createdAnswers,
                Text = createQuestionDTO.Text,
                TextPl = createQuestionDTO.TextPl,
                Category = await _categoryRepository.GetAsync(c => c.Id == createQuestionDTO.CategoryId),
                CorrectAnswerId = answers.FirstOrDefault(a => a.Text == createQuestionDTO.CorrectAnswerText).Id                
            };
            var createdQuestion = await _questionRepository.CreateAsync(question);
            await _questionRepository.SaveAsync();
            return createdQuestion;
        }


        public async Task<ICollection<Question>> GetAllQuestions()
        {
            return await _questionRepository.GetAllAsync();
        }


        public async Task DeleteQuestionById(int questionId)
        {
            var question = await _questionRepository.GetAsync(q => q.Id == questionId);
            if (question == null)
            {
                throw new Exception("There is no question with that id");
            }
            var answers = question.Answers;
            await _questionRepository.RemoveAsync(question);
            await _questionRepository.SaveAsync();
            foreach (var answer in answers)
            {
                await _answerRepository.RemoveAsync(answer);
            }            
            await _questionRepository.SaveAsync();
        }


        public async Task<ICollection<Question>> GetQuestionsByCategory(string categoryName)
        {
            var category = await _categoryRepository.GetAsync(c => c.Name == categoryName);
            if (category == null)
            {
                throw new Exception("There is no category with this name: " + categoryName);
            }
            var questions = await _questionRepository.GetAllAsync(q => q.Category == category);
            return questions;
        }

        public async Task<ICollection<Question>> GetRandomQuestionsByCategory(int range, string categoryName)
        {
            var category = await _categoryRepository.GetAsync(c => c.Name == categoryName);
            if(category == null)
            {
                throw new Exception("There is no category with this name: " + categoryName);
            }
            var allQuestionsByCategory = await GetQuestionsByCategory(categoryName);
            var random = new Random();
            var questionsList = new List<Question>(allQuestionsByCategory);
            var randomQuestions = new List<Question>();
            while(randomQuestions.Count < range)
            {
                int randomIndex = random.Next(questionsList.Count);
                if (!randomQuestions.Contains(questionsList[randomIndex])){
                    randomQuestions.Add(questionsList[randomIndex]);
                }
            }
            return randomQuestions;            
        }
    }
}
