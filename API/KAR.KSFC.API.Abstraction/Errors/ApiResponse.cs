using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Abstraction.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStausCode(statusCode);
        }

        private string GetDefaultMessageForStausCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                403 => "Restriced , you are not",
                404 => "Resource found, it was not",
                500 => "Error are the path of dark side. Error leads anger, Anger leads to hate , Hate leads to career change",
                _ => null
            };
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
