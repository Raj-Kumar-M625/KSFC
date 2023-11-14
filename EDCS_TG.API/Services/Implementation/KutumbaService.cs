using EDCS_TG.API.Helpers.Kutumba;
using EDCS_TG.API.Services.Interfaces;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class KutumbaService : IKutumbaService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitofwork;

        public KutumbaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ResultDataList>> GetKutumbaData(KutumbaRequestDto requestDto)
        {
            var url = _configuration["Kutumba:URL"];
            var key = _configuration["Kutumba:AESKey"];
            var iv = _configuration["Kutumba:IVKey"];
            var secretKey = _configuration["Kutumba:SecretKey"];
            var clientCode = _configuration["Kutumba:ClientCode"];
            requestDto.ClientCode = clientCode;
            if (requestDto.Aadhar_No != "")
            {
                var hashAadhar = HashAadhar(requestDto.Aadhar_No);
                requestDto.Aadhar_No = hashAadhar;
            }
            var hmacString = StringHMAC(requestDto);
            var hashHMACHex = HashHMACHex(secretKey, hmacString);
            requestDto.HashedMac = hashHMACHex;

            //Get Response from Kutumba API
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = client.PostAsJsonAsync(url, requestDto).Result;
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            KutumbaEncryptedResponseDto responseMapping = JsonConvert.DeserializeObject<KutumbaEncryptedResponseDto>(responseText);
           
            List<ResultDataList> data = new List<ResultDataList>();
            if (responseMapping.StatusCode == 0)
            {
                var decryptedDataString = DecryptString(key, iv, responseMapping.EncResultData);
                
                KutumbaResponseDto decryptedData = JsonConvert.DeserializeObject<KutumbaResponseDto>(decryptedDataString);
                data = decryptedData.ResultDataList;

            }
          
            return data;
        }

        public string DecryptString(string key, string IV, string cipherText)
        {
            byte[] buffer = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new
                   CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new
                       StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        internal string StringHMAC(KutumbaRequestDto requestDto)
        {
            string inputForHMAC = requestDto.ClientCode + "_"
                                + requestDto.BenID + "_"
                                + requestDto.RC_Number + "_"
                                + requestDto.Aadhar_No + "_"
                                + requestDto.Mobile_No;
            return inputForHMAC;
        }

        internal string HashHMACHex(string hMACKey, string InputValue)
        {
            string hashHMACHex = string.Empty;
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] keyByte = encoding.GetBytes(hMACKey);
                HMACSHA256 hmacsha1 = new HMACSHA256(keyByte);
                byte[] messageBytes = encoding.GetBytes(InputValue);
                byte[] hash = HashHMAC(keyByte, messageBytes);
                hashHMACHex = HashEncode(hash);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return hashHMACHex;
        }
        internal string HashAadhar(string cardNumber)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(cardNumber));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash.ToUpper();
        }
        private byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }
        private string HashEncode(byte[] hash)
        {
            return Convert.ToBase64String(hash);
        }
    }
}
