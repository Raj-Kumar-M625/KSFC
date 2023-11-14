using EDCS_TG.API.Data.Models;

namespace EDCS_TG.API.Data.Repository.Interfaces
{
    public interface IQuestionPaperRepository : IRepository<QuestionPaper>
    {
        Task<IEnumerable<QuestionPaper>> GetAllQuestionPapers();
    }
}