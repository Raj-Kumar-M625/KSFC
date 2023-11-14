using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IPersonalDetailsRepository? _personalDetailsRepository;
        private ISocialSecurityRepository? _socialSecurityRepository;
        private IEducationRepository? _educationRepository;
        private IEmploymentRepository _employmentRepository;
        private IHousingRepository? _housingRepository;
        private IHealthRepository? _healthRepository;
        private IAdditionalInformationRepository? _additionalInformationRepository;
        private IBasicSurveylDetailRepository? _basicSurveylDetailRepository;
        private IQuestionRepository? _questionRepository;
        private readonly KarmaniDbContext _karmaniDbContext;
        private ISurveyRepository? _surveyRepository;
        private IOfficeRepository? _officeRepository;
        private IUserAssignmentREpository? _userAssignmentREpository;
        private IUserRepository? _userRepository;
        private IQuestionPaperRepository? _questionPaper;
        private IQuestionPaperQuestionRepository? _questionPaperQuestion;
        private IQuestionPaperAnswerRepository? _questionPaperAnswer;
        private ISurveyImagesRepository _surveyImagesRepository;
        private IDBTEKYCDataRepository _dBTEKYCDataRepository;
        private IDBTEKYCRequestRepository _dBTEKYCRequestRepository;
        private IDBTEKYCResponseRepository _dBTEKYCResponseRepository;
        private IDBTLocalEKYCDataRepository _dBTLocalEKYCDataRepository;
        private IKutumbaDLRepository? _kutumbaDLRepository;
        private ISMSLogRepository? _sMSLog;

        public UnitOfWork(KarmaniDbContext karmaniDbContext)
        {
            _karmaniDbContext = karmaniDbContext;
        }
        public IUserRepository UserRepository

        {
            get { return _userRepository ??= new UserRepository(_karmaniDbContext); }
        }
        public IPersonalDetailsRepository PersonalDetailsRepository
        {
            get { return _personalDetailsRepository ??= new PersonalDetailsRepository(_karmaniDbContext); }
        }

        public IEducationRepository EducationRepository
        {
            get { return _educationRepository ??= new EducationRepository(_karmaniDbContext); }
        }

        public IEmploymentRepository EmploymentRepository
        {
            get { return _employmentRepository ??= new EmploymentRepository(_karmaniDbContext); }
        }

        public IHousingRepository HousingRepository

        {
            get { return _housingRepository ??= new HousingRepository(_karmaniDbContext); }
        }

        public IHealthRepository HealthRepository
        {
            get { return _healthRepository ??= new HealthRepository(_karmaniDbContext); }
        }

        public ISocialSecurityRepository SocialSecurityRepository
        {
            get { return _socialSecurityRepository ??= new SocialSecurityRepository(_karmaniDbContext); }   
        }

        public IAdditionalInformationRepository AdditionalInformationRepository
        {
            get { return _additionalInformationRepository ??= new AdditionalInformationRepository(_karmaniDbContext); }
        }

        public IBasicSurveylDetailRepository BasicSurveyDetailRepository
        {
            get { return _basicSurveylDetailRepository ??= new BasicSurveyDetailRepository(_karmaniDbContext); }
        }

        public IQuestionRepository questionRepository
        {
            get { return _questionRepository ??= new QuestionRepository(_karmaniDbContext); }
        }

        public ISurveyRepository surveyRepository
        {
            get { return _surveyRepository ??= new SurveyRepository(_karmaniDbContext); }   
        }

        public IOfficeRepository officeRepository
        {
            get { return _officeRepository ??= new OfficeRepository(_karmaniDbContext); }
        }

        public IUserAssignmentREpository UserAssignmentREpository
        {
            get { return _userAssignmentREpository ??= new UserAssignmentRepository(_karmaniDbContext); }
        }

        public IDBTEKYCDataRepository dBTEKYCDataRepository 
        { 
            get { return _dBTEKYCDataRepository ??= new DBTEKYCDataRepository(_karmaniDbContext); }
        }

        public IDBTEKYCRequestRepository dBTEKYCRequestRepository
        {
            get { return _dBTEKYCRequestRepository ??= new DBTEKYCRequestRepository(_karmaniDbContext); }
        }

        public IDBTEKYCResponseRepository dBTEKYCResponseRepository
        {
            get { return _dBTEKYCResponseRepository ??= new DBTEKYCResponseRepository(_karmaniDbContext); }

        }

        public IDBTLocalEKYCDataRepository dBTLocalEKYCDataRepository
        {
            get { return _dBTLocalEKYCDataRepository ??= new DBTLocalEKYCDataRepository(_karmaniDbContext); }
        }

        public ISurveyImagesRepository SurveyImagesRepository
        {
            get { return _surveyImagesRepository ??= new SurveyImagesRepository(_karmaniDbContext); }

        }

        public IQuestionPaperRepository QuestionPaper
        {
            get { return _questionPaper ??= new QuestionPaperRepository(_karmaniDbContext); }
        }

        public IQuestionPaperQuestionRepository QuestionPaperQuestion
        {
            get { return _questionPaperQuestion ??= new QuestionPaperQuestionRepository(_karmaniDbContext); }
        }

        public IQuestionPaperAnswerRepository QuestionPaperAnswer
        {
            get { return _questionPaperAnswer ??= new QuestionPaperAnswerRepository(_karmaniDbContext); }
        }

        public IKutumbaDLRepository KutumbaDL
        { 
            get { return _kutumbaDLRepository ??= new KutumbaDLRepository(_karmaniDbContext);}
        }

        public ISMSLogRepository SMSLog
        {
            get { return _sMSLog ??= new SMSLogRepository(_karmaniDbContext); }
        }

        public void Dispose()
        {
            _karmaniDbContext.Dispose();
        }

        public int save()
        {
            return _karmaniDbContext.SaveChanges();
        }
    }
}
