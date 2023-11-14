namespace EDCS_TG.API.DTO
{
    public class DownloadQuestionPaperDto : MinimumResponseDto
    {
        public IEnumerable<QuestionPaperDto>? QuestionPapers { get; set; }
    }
}
