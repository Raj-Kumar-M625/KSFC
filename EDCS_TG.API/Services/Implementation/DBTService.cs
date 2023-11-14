using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO.DBT;
using EDCS_TG.API.Services.Interfaces;
using Newtonsoft.Json;

namespace EDCS_TG.API.Services.Implementation
{
    public class DBTService : IDBTService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitofwork;
        public DBTService(IConfiguration configuration, IMapper mapper, IUnitOfWork unitofwork)
        {
            _configuration = configuration;
            _mapper = mapper;
            _unitofwork = unitofwork;
        }

        public async Task<string> EKyc(string name, int SurveyeeId)
        {
            var tokenURL = _configuration["DBT:TokenURL"];
            var departmentCode = _configuration["DBT:DepartmentCode"];
            var applicationCode = _configuration["DBT:ApplicationCode"];
            var schemeCode = _configuration["DBT:SchemeCode"];
            var integrationKey = _configuration["DBT:IntegrationKey"];
            var integrationPassword = _configuration["DBT:IntegrationPassword"];
            var serviceCode = _configuration["DBT:ServiceCode"];
            var redirectionURL = _configuration["DBT:RedirectionURL"];
            var date = DateTime.Now;

            var dBTRegisterRequest = new DBTRegisterRequestDto()
            {
                DeptCode = departmentCode,
                ApplnCode = applicationCode,
                SchemeCode = schemeCode,
                BeneficiaryID = new Random().Next(500000, 999999).ToString(),
                BeneficiaryName = name,
                IntegrationKey = integrationKey,
                IntegrationPassword = integrationPassword,
                TxnNo = new Random().Next(500000, 999999).ToString(),
                TxnDateTime = String.Format("{0:yyyyMMddHHmmss}", date),
                ServiceCode = serviceCode,
                ResponseRedirectURL = redirectionURL,
            };

            //Save data in table for reference
            var newRequest = _mapper.Map<DBTEKYCRequest>(dBTRegisterRequest);
            newRequest.BasicSurveyId = SurveyeeId;
            var data = await _unitofwork.dBTEKYCRequestRepository.Create(newRequest);

            //Get Token from DBT
            using HttpClient client = new();
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = client.PostAsJsonAsync(tokenURL, dBTRegisterRequest).Result;
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            DBTRegisterResponseDto responseMapping = JsonConvert.DeserializeObject<DBTRegisterResponseDto>(responseText);

            if (responseMapping != null)
            {
                var aadharURL = GenerateAadharURL(integrationKey, responseMapping.Token);
                return aadharURL;
            }

            return "";
        }

        public async Task<int> GetKYCResponse(string vaultrefno)
        {
            var kYCResponse = await _unitofwork.dBTEKYCResponseRepository.FindById(data => data.VaultRefNumber == vaultrefno);

            var requestData = await _unitofwork.dBTEKYCRequestRepository.FindById(request => request.TxnNo == kYCResponse.TxnNo);

            return requestData.BasicSurveyId;
        }

        public async Task<KYCResponse> KYCResponse(KYCResponse kYCResponse)
        {
            var newEkycResponse = _mapper.Map<DBTEKYCResponse>(kYCResponse);
            var ekycResponse = await _unitofwork.dBTEKYCResponseRepository.Create(newEkycResponse);

            var newEkycData = _mapper.Map<DBTEKYCData>(kYCResponse.eKYCData);
            newEkycData.DBTEKYCResponseId = ekycResponse.Id;
            var ekycData = await _unitofwork.dBTEKYCDataRepository.Create(newEkycData);

            var newEkycLocalData = _mapper.Map<DBTLocalEKYCData>(kYCResponse.localKYCData);
            newEkycLocalData.DBTEKYCResponseId = ekycResponse.Id;
            var ekycLocalData = await _unitofwork.dBTLocalEKYCDataRepository.Create(newEkycLocalData);

            var data = _mapper.Map<KYCResponse>(ekycResponse);
            data.eKYCData = _mapper.Map<EKYCData>(ekycData);
            data.localKYCData = _mapper.Map<LocalKYCData>(ekycLocalData);
            return data;
        }

        // Generate e-KYC URL
        public string GenerateAadharURL(string key, string token)
        {
            var baseURL = _configuration["DBT:AadharURL"];
            string URL = baseURL + "?key=" + key + "&token=" + token;

            return URL;
        }
    }
}
