using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Mapper
{
    public class CustomMapper:Profile
    {
        public CustomMapper()
        {
            CreateMap<PersonalDetails, PersonalDetailsDto>().ReverseMap();
            CreateMap<Education, EducationDto>().ReverseMap();
            CreateMap<Employment, EmploymentDto>().ReverseMap();
            CreateMap<Housing, HousingDto>().ReverseMap();
            CreateMap<Health, HealthDto>().ReverseMap();
            CreateMap<SocialSecurity,SocialSecurityDto>().ReverseMap();
            CreateMap<AdditionalInformation, AdditionalInformationDto>().ReverseMap();  
            CreateMap<BasicSurveyDetail, BasicSurveyDetailDto>().ReverseMap();
            CreateMap<Questions, QuestionsDto>().ReverseMap();
            CreateMap<Survey, SurveyDto>().ReverseMap();    
            CreateMap<Office,OfficeDto>().ReverseMap();
            CreateMap<QuestionPaper, QuestionPaperDto>().ReverseMap();
            CreateMap<QuestionPaperQuestion, QuestionPaperQuestionDto>().ReverseMap();
            CreateMap<QuestionPaperAnswer, QuestionPaperAnswerDto>().ReverseMap();


        }
    }
}
