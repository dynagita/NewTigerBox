using System.Collections.Generic;
using System.Net;

namespace NewTigerBox.Common.Response
{
    public class Output
    {
        public HttpStatusCode StatusCode { get; private set; }
        public List<string> ErrorMessages { get; private set; }
        public object Data { get; private set; }
        public bool IsValid { get; private set; }

        private Output(object data)
        {
            Data = data;
            StatusCode = HttpStatusCode.OK;
            IsValid = true;
        }

        private Output(string errorMessage)
        {
            StatusCode = HttpStatusCode.BadRequest;
            ErrorMessages = new List<string>()
            {
                errorMessage
            };
            IsValid = false;
        }

        private Output(List<string> errorMessages)
        {
            StatusCode = HttpStatusCode.BadRequest;
            ErrorMessages = errorMessages;
            IsValid = false;
        }

        private Output() { }

        public static Output ValidResult() => new Output() { IsValid = true};
        public static Output ValidResult(object data) => new Output() { IsValid = true, Data = data };
        public static Output InvalidResult(string error) => new Output() { IsValid = false, ErrorMessages =  new List<string>() { error } };
        public static Output Ok(object data) => new Output(data);
        public static Output BadRequest(string error) => new Output(error);
        public static Output BadRequest(List<string> errors) => new Output(errors);
        public static Output InternalServerError() 
            => new Output
            { 
                StatusCode = HttpStatusCode.InternalServerError, 
                ErrorMessages = new List<string> { "Our servers are facing troubles, please try again in few minutes. If this error persist, please contact us." },
                IsValid = false
            };
    }
}
