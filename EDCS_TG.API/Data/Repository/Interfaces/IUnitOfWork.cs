namespace EDCS_TG.API.Data.Repository.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IKutumbaDLRepository KutumbaDL { get; }
        IUserRepository UserRepository { get; }

        IPersonalDetailsRepository PersonalDetailsRepository { get; }
        IEducationRepository EducationRepository { get; }

        IEmploymentRepository EmploymentRepository { get; }
        IHousingRepository HousingRepository { get; }
        IHealthRepository HealthRepository { get; }

        ISocialSecurityRepository SocialSecurityRepository { get; }

        IAdditionalInformationRepository AdditionalInformationRepository { get; }

        IBasicSurveylDetailRepository BasicSurveyDetailRepository { get; }
        IQuestionRepository questionRepository { get; }
        ISurveyRepository surveyRepository { get; } 

        IOfficeRepository officeRepository { get; }

        ISurveyImagesRepository SurveyImagesRepository { get; }
        IUserAssignmentREpository UserAssignmentREpository { get; }
        IDBTEKYCDataRepository dBTEKYCDataRepository { get; }

        IDBTEKYCRequestRepository dBTEKYCRequestRepository { get; }

        IDBTEKYCResponseRepository dBTEKYCResponseRepository { get; }

        IDBTLocalEKYCDataRepository dBTLocalEKYCDataRepository { get; }
        IQuestionPaperRepository QuestionPaper { get; }
        IQuestionPaperQuestionRepository QuestionPaperQuestion { get; }
        IQuestionPaperAnswerRepository QuestionPaperAnswer { get; }
        ISMSLogRepository SMSLog { get; }
        //object QuestionPaper { get; }

        int save();
    }
}
