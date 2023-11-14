using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository;

public class QuestionPaperAnswerRepository : Repository<QuestionPaperAnswer>, IQuestionPaperAnswerRepository
{
    public QuestionPaperAnswerRepository(KarmaniDbContext karmanidbcontex) : base(karmanidbcontex)
    {
    }
}