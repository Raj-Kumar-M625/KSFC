using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace EDCS_TG.API.Data.Repository;

public class QuestionPaperRepository : Repository<QuestionPaper>, IQuestionPaperRepository
{
    public QuestionPaperRepository(KarmaniDbContext karmanidbcontex) : base(karmanidbcontex)
    {
    }

    public async Task<IEnumerable<QuestionPaper>> GetAllQuestionPapers()
    {
        var data = await KarmaniDbContext.QuestionPaper
                       .Where(x => x.IsActive).Include
                       (QuestionPaper => QuestionPaper.QuestionPaperQuestions)
                       .ThenInclude(QuestionPaperQuestion => QuestionPaperQuestion.QuestionPaperAnswers)
                       .ToListAsync();

        return data;
    }
}