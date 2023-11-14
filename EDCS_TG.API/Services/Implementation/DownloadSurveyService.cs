using AspNetCoreRateLimit;
using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class DownloadSurveyService : IDownloadSurveyService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitofwork;

        public DownloadSurveyService(IMapper mapper, IUnitOfWork unitofwork)
        {
            _mapper = mapper;
            _unitofwork = unitofwork;
        }

        public async Task<IEnumerable<DownloadSurveyModel>> GetAllDownload()
        {
            var surveyAnswer = await _unitofwork.surveyRepository.FindAll();
            var surveyUser = await _unitofwork.UserRepository.FindAll();
            var surveyBasicDetails = await _unitofwork.BasicSurveyDetailRepository.FindAll();
            var QuestionPaperQuestion = await _unitofwork.QuestionPaperQuestion.FindAll();


            var result = (
                            from s in surveyAnswer
                            join u in surveyUser on s.UserId equals u.Id
                            join bsd in surveyBasicDetails on s.SurveyId equals bsd.SurveyId
                            join qpq in QuestionPaperQuestion on s.QuestionId equals qpq.Id
                            where bsd.Status == "Completed"
                            select new
                            {
                                SurveyorID = u.SurveyorId,
                                SurveyorName = u.UserName,
                                SurveyId = s.SurveyId,
                                Name = bsd.Name,
                                SubCategory = qpq.SubCategoryName,
                                QText = qpq.QText,
                                Answer = s.Answer,
                                CreatedDate = s.Created_Date,
                                Status = bsd.Status
                            }).Union(
                            from b in surveyBasicDetails
                            join u in surveyUser on b.UserId equals u.Id
                            let unpivoted = new unpivoted[]
                            {
                                   new unpivoted{ ColumnName = "Name", ColumnValue = b?.Name },
                                   new unpivoted{ ColumnName = "DOB", ColumnValue =  @Convert.ToDateTime(b.DOB).ToString("dd/MM/yyyy")},
                                   new unpivoted{ ColumnName = "Age", ColumnValue = b.Age.ToString() },
                                   new unpivoted{ ColumnName = "GenderByBirth", ColumnValue = b.GenderByBirth },
                                   new unpivoted{ ColumnName = "MobileNumber", ColumnValue = b.number.ToString() },
                                   new unpivoted{ ColumnName = "Email", ColumnValue = b.Email },
                                   new unpivoted{ ColumnName = "Address", ColumnValue = b.Address },
                                   new unpivoted{ ColumnName = "District", ColumnValue = b.District },
                                   new unpivoted{ ColumnName = "Taluk", ColumnValue = b.Taluk },
                                   new unpivoted{ ColumnName = "Hobli", ColumnValue = b.Hobli },
                                   new unpivoted{ ColumnName = "Village", ColumnValue = b.VillageOrWard },
                                   new unpivoted{ ColumnName = "Pincode", ColumnValue = b.PinCode.ToString() },
                                   new unpivoted{ ColumnName = "Present Address", ColumnValue = b.PresentAddress },
                                   new unpivoted{ ColumnName = "Present District", ColumnValue = b.PresentDistrict },
                                   new unpivoted{ ColumnName = "Present Taluk", ColumnValue = b.PresentTaluk },
                                   new unpivoted{ ColumnName = "Present Hobli", ColumnValue = b.PresentHobli },
                                   new unpivoted{ ColumnName = "Present Village or Ward", ColumnValue = b.PresentVillageOrWard },
                                   new unpivoted{ ColumnName = "Present Pincode", ColumnValue = b.PresentPinCode },
                                   new unpivoted{ ColumnName = "Aadhar Status", ColumnValue = b.AadharStatus }
                                }
                            from up in unpivoted
                            select new
                            {
                                SurveyorID = u.SurveyorId,
                                SurveyorName = u.UserName,
                                SurveyId = b.SurveyId,
                                Name = b.Name,
                                SubCategory = "0.BasicDetails",
                                QText = up.ColumnName,
                                Answer = up.ColumnValue,
                                CreatedDate = b.Created_Date,
                                Status = b.Status
                            }).Where(b => b.Status == "Completed").ToList();
            var DownloadResult = _mapper.Map<IEnumerable<DownloadSurveyModel>>(result);
            return DownloadResult;

        }


        public async Task<IEnumerable<DownloadSurveyModel>> GetIndividualSurveyDownload(string SurveyId)
        {
            var surveyAnswer = await _unitofwork.surveyRepository.FindByCondition(item => item.SurveyId == SurveyId);
            var surveyUser = await _unitofwork.UserRepository.FindAll();
            var surveyBasicDetails = await _unitofwork.BasicSurveyDetailRepository.FindByCondition(item => item.SurveyId == SurveyId);
            var QuestionPaperQuestion = await _unitofwork.QuestionPaperQuestion.FindAll();

            var result = (
                      from s in surveyAnswer
                      join u in surveyUser on s.UserId equals u.Id
                      join bsd in surveyBasicDetails on s.SurveyId equals bsd.SurveyId
                      join qpq in QuestionPaperQuestion on s.QuestionId equals qpq.Id
                      select new DownloadSurveyModel
                      {
                          SurveyorId = u.SurveyorId,
                          SurveyorName = u.UserName,
                          SurveyId = s.SurveyId,
                          Name = bsd.Name,
                          SubeCategoryName = qpq.SubCategoryName,
                          QText = qpq.QText,
                          Answer = s.Answer,
                          CreatedDate = @Convert.ToDateTime(s.Created_Date).ToString("dd/MM/yyyy"),
                          Status = bsd.Status
                      }).Union(
                       from b in surveyBasicDetails
                       join u in surveyUser on b.UserId equals u.Id
                       let unpivoted = new unpivoted[]
                       {
                         new unpivoted{ ColumnName = b.Locale == "en"?"Name":"ಹೆಸರು", ColumnValue = b?.Name },
                         new unpivoted{ ColumnName = b.Locale == "en"?"DOB":"ಹುಟ್ಟಿದ ದಿನ", ColumnValue =  @Convert.ToDateTime(b.DOB).ToString("dd/MM/yyyy") },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Age":"ವಯಸ್ಸು", ColumnValue = b.Age.ToString() },
                         new unpivoted{ ColumnName = b.Locale == "en"?"GenderByBirth":"ಲಿಂಗ", ColumnValue = b.GenderByBirth },
                         new unpivoted{ ColumnName = b.Locale == "en"?"MobileNumber":"ಮೊಬೈಲ್ ನಂಬರ", ColumnValue = b.number.ToString() },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Email":"ಇಮೇಲ್", ColumnValue = b.Email },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Address":"ಶಾಶ್ವತ ವಿಳಾಸ", ColumnValue = b.Address },
                         new unpivoted{ ColumnName = b.Locale == "en"?"District":"ಜಿಲ್ಲೆ", ColumnValue = b.District },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Taluk":"ತಾಲ್ಲೂಕು/ಪಟ್ಟಣ", ColumnValue = b.Taluk },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Hobli":"ಹೋಬಳಿ/ವಲಯ", ColumnValue = b.Hobli },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Village":"ಗ್ರಾಮ/ವಾರ್ಡ್", ColumnValue = b.VillageOrWard },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Pincode":"ಪಿನ್ ಕೋಡ್", ColumnValue = b.PinCode.ToString() },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Address":"ಈಗಿನ ವಿಳಾಸ", ColumnValue = b.PresentAddress },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present District":"ಈಗಿನ ಜಿಲ್ಲೆ", ColumnValue = b.PresentDistrict },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Taluk":"ಈಗಿನ ತಾಲ್ಲೂಕು/ಪಟ್ಟಣ", ColumnValue = b.PresentTaluk },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Hobli":"ಈಗಿನ ಹೋಬಳಿ/ವಲಯ", ColumnValue = b.PresentHobli },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Village or Ward":"ಈಗಿನ ಗ್ರಾಮ/ವಾರ್ಡ್", ColumnValue = b.PresentVillageOrWard },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Pincode":"ಈಗಿನ ಪಿನ್ ಕೋಡ್", ColumnValue = b.PresentPinCode },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Aadhar Status":"ಆಧಾರ್ ಸ್ಥಿತಿ", ColumnValue = b.AadharStatus }
                       }
                       from up in unpivoted
                       select new DownloadSurveyModel
                       {
                           SurveyorId = u.SurveyorId,
                           SurveyorName = u.UserName,
                           SurveyId = b.SurveyId,
                           Name = b.Name,
                           SubeCategoryName = b.Locale == "en" ? "0.BasicDetails" : "0.ಮೂಲ ವಿವರಗಳು",
                           QText = up.ColumnName,
                           Answer = up.ColumnValue,
                           CreatedDate = @Convert.ToDateTime(b.Created_Date).ToString("dd/MM/yyyy"),
                           Status = b.Status
                       }
                      ).OrderBy(x => x.SubeCategoryName);

            var DownloadResult = _mapper.Map<IEnumerable<DownloadSurveyModel>>(result);
            return DownloadResult;
        }

        public async Task<IEnumerable<DownloadSurveyModel>> FilterSurveyDownload(SearchFilter filter)
        {
            IEnumerable<BasicSurveyDetail> surveyBasicDetails = await _unitofwork.BasicSurveyDetailRepository.FindAll();
            Office office;

            if (filter.District != null)
            {
                office = await _unitofwork.officeRepository.FindById(x => x.DistrictName == filter.District || x.DistrictName_KA == filter.District);
                surveyBasicDetails = surveyBasicDetails.Where(x => x.District == office.DistrictName || x.District == office.DistrictName_KA).ToList();
            }

            if (filter.Taluk != null)
            {
                office = await _unitofwork.officeRepository.FindById(x => x.TalukOrTownName == filter.Taluk || x.TalukOrTownName_KA == filter.Taluk);
                surveyBasicDetails = surveyBasicDetails.Where(x => x.Taluk == office.TalukOrTownName || x.Taluk == office.TalukOrTownName_KA).ToList();
            }

            if (filter.Hobli != null)
            {
                office = await _unitofwork.officeRepository.FindById(x => x.HobliOrZoneName == filter.Hobli || x.HobliOrZoneName_KA == filter.Hobli);
                surveyBasicDetails = surveyBasicDetails.Where(x => x.Hobli == office.HobliOrZoneName || x.Hobli == office.HobliOrZoneName_KA).ToList();
            }

            if (filter.Village != null)
            {
                office = await _unitofwork.officeRepository.FindById(x => x.VillageOrWardName == filter.Village || x.VillageOrWardName_KA == filter.Village);
                surveyBasicDetails = surveyBasicDetails.Where(x => x.VillageOrWard == office.VillageOrWardName || x.VillageOrWard == office.VillageOrWardName_KA).ToList();
            }

            if (filter.SurveyeeName != null)
            {
                surveyBasicDetails = surveyBasicDetails.Where(x => (bool)x.Name.Contains(filter.SurveyeeName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (filter.SurveyorName != null)
            {
                surveyBasicDetails = surveyBasicDetails.Where(x => (bool)x.User?.UserName.Contains(filter.SurveyorName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (filter.SurveyId != null)
            {
                surveyBasicDetails = surveyBasicDetails.Where(x => x.SurveyId.Contains(filter.SurveyId, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (filter.StartDate != null && filter.EndDate != null)
            {
                surveyBasicDetails = surveyBasicDetails.Where(x => x.Created_Date >= filter.StartDate && x.Created_Date <= filter.EndDate).ToList();
            }

            if (filter.Status != null)
            {
                if (filter.Status == "Completed")
                {
                    surveyBasicDetails = surveyBasicDetails.Where(x => x.Status == filter.Status).ToList();
                }
                else
                {
                    surveyBasicDetails = surveyBasicDetails.Where(x => x.Status == null).ToList();
                }
            }

            var allSurveyAnswer = await _unitofwork.surveyRepository.FindAll();

            var surveyAnswer = (from asa in allSurveyAnswer
                                join sbd in surveyBasicDetails on
                                asa.SurveyId equals sbd.SurveyId
                                select asa).ToList();

            var surveyUser = await _unitofwork.UserRepository.FindAll();
            var QuestionPaperQuestion = await _unitofwork.QuestionPaperQuestion.FindAll();


            var result = (
                from b in surveyBasicDetails
                join u in surveyUser on b.UserId equals u.Id
                let unpivoted = new unpivoted[]
              {
                         new unpivoted{ ColumnName = b.Locale == "en"?"Name":"ಹೆಸರು", ColumnValue = b?.Name },
                         new unpivoted{ ColumnName = b.Locale == "en"?"DOB":"ಹುಟ್ಟಿದ ದಿನ", ColumnValue = @Convert.ToDateTime(b.DOB).ToString("dd/MM/yyyy") },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Age":"ವಯಸ್ಸು", ColumnValue = b.Age.ToString() },
                         new unpivoted{ ColumnName = b.Locale == "en"?"GenderByBirth":"ಲಿಂಗ", ColumnValue = b.GenderByBirth },
                         new unpivoted{ ColumnName = b.Locale == "en"?"MobileNumber":"ಮೊಬೈಲ್ ನಂಬರ", ColumnValue = b.number.ToString() },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Email":"ಇಮೇಲ್", ColumnValue = b.Email },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Address":"ಶಾಶ್ವತ ವಿಳಾಸ", ColumnValue = b.Address },
                         new unpivoted{ ColumnName = b.Locale == "en"?"District":"ಜಿಲ್ಲೆ", ColumnValue = b.District },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Taluk":"ತಾಲ್ಲೂಕು/ಪಟ್ಟಣ", ColumnValue = b.Taluk },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Hobli":"ಹೋಬಳಿ/ವಲಯ", ColumnValue = b.Hobli },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Village":"ಗ್ರಾಮ/ವಾರ್ಡ್", ColumnValue = b.VillageOrWard },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Pincode":"ಪಿನ್ ಕೋಡ್", ColumnValue = b.PinCode.ToString() },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Address":"ಈಗಿನ ವಿಳಾಸ", ColumnValue = b.PresentAddress },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present District":"ಈಗಿನ ಜಿಲ್ಲೆ", ColumnValue = b.PresentDistrict },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Taluk":"ಈಗಿನ ತಾಲ್ಲೂಕು/ಪಟ್ಟಣ", ColumnValue = b.PresentTaluk },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Hobli":"ಈಗಿನ ಹೋಬಳಿ/ವಲಯ", ColumnValue = b.PresentHobli },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Village or Ward":"ಈಗಿನ ಗ್ರಾಮ/ವಾರ್ಡ್", ColumnValue = b.PresentVillageOrWard },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Present Pincode":"ಈಗಿನ ಪಿನ್ ಕೋಡ್", ColumnValue = b.PresentPinCode },
                         new unpivoted{ ColumnName = b.Locale == "en"?"Aadhar Status":"ಆಧಾರ್ ಸ್ಥಿತಿ", ColumnValue = b.AadharStatus }
              }
                from up in unpivoted
                select new DownloadSurveyModel
                {
                    SurveyorId = u.SurveyorId,
                    SurveyorName = u.UserName,
                    SurveyId = b.SurveyId,
                    Name = b.Name,
                    SubeCategoryName = b.Locale == "en" ? "0.BasicDetails" : "0.ಮೂಲ ವಿವರಗಳು",
                    QText = up.ColumnName,
                    Answer = up.ColumnValue,
                    CreatedDate = @Convert.ToDateTime(b.Created_Date).ToString("dd/MM/yyyy"),
                    Status = b.Status
                }).Union(
                from s in surveyAnswer
                join u in surveyUser on s.UserId equals u.Id
                join bsd in surveyBasicDetails on s.SurveyId equals bsd.SurveyId
                join qpq in QuestionPaperQuestion on s.QuestionId equals qpq.Id
                select new DownloadSurveyModel
                {
                    SurveyorId = u.SurveyorId,
                    SurveyorName = u.UserName,
                    SurveyId = s.SurveyId,
                    Name = bsd.Name,
                    SubeCategoryName = qpq.SubCategoryName,
                    QText = qpq.QText,
                    Answer = s.Answer,
                    CreatedDate = @Convert.ToDateTime(s.Created_Date).ToString("dd/MM/yyyy"),
                    Status = bsd.Status
                }).OrderBy(x => x.SurveyId);

            var DownloadResult = _mapper.Map<IEnumerable<DownloadSurveyModel>>(result);
            return DownloadResult;
        }
    }
    public class unpivoted
    {
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }

    }


}
