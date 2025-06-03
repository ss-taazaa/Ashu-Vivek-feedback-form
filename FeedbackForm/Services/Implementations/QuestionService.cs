//using FeedbackForm.Models;
//using FeedbackForm.Repositories.Interfaces;
//using FeedbackForm.Services.Interfaces;

//namespace FeedbackForm.Services.Implementations
//{
//    public class QuestionService : IQuestion
//    {
//        private readonly IGenericRepository<Question> _questionRepository;

//        public QuestionService(IGenericRepository<Question> questionRepository)
//        {
//            _questionRepository = questionRepository;
//        }


//        public async Task<IEnumerable<Question>> GetAllQuestionAsync()
//        {
//            return await _questionRepository.GetAllAsync();
//        }


//        public async Task<Question> GetQuestionByIdAsync(Guid id)
//        {
//            var existingQuestion = await _questionRepository.GetByIdAsync(id);
//            return existingQuestion;
//        }



//        public async Task<Question> UpdateQuestionByIdAsync(Guid id, Question question)
//        {
//            var existingQuestion = _questionRepository.GetByIdAsync(id);

//            if (existingQuestion != null)
//            {
//                return null;
//            }
//            //return 
//        }


//        public async Task<Question> CreateQuestionAsync(Question question)
//        {
//            question.Id = Guid.NewGuid();
//            return await _questionRepository.AddAsync(question);
//        }




//        public async Task<bool> DeleteQuestionAsync(Guid id)
//        {
//            var existingQuestion =await _questionRepository.GetByIdAsync(id);
//            if (existingQuestion == null)
//            {
//                return false;
//            }
//            _questionRepository.Remove(existingQuestion);
//            return true;
//        }
//    }
//}
