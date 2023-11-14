using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository;

public class QuestionPaperQuestionRepository : Repository<QuestionPaperQuestion>, IQuestionPaperQuestionRepository
{
    public QuestionPaperQuestionRepository(KarmaniDbContext karmanidbcontex) : base(karmanidbcontex)
    {
    }
}