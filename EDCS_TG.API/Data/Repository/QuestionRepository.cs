using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class QuestionRepository: Repository<Questions>, IQuestionRepository
    {
        public QuestionRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }
    }
}
