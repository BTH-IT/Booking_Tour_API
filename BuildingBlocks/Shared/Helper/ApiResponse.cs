using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.Helper
{
    public class ApiResponse<T>
    {
        public ApiResponse(int statusCode, T? result, string message) 
            => (StatusCode, Result, Message) = (statusCode, result, message);

        public ApiResponse(ModelStateDictionary modelState,T result ) 
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }
            var Errors = string.Join(",", modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray());
            if (!string.IsNullOrEmpty(this.Message))
                this.Message += ", " + Errors;
            else this.Message = Errors;
            this.StatusCode = 400;
            this.Result = result;
        }

        public int StatusCode { get; set; }
        public T? Result { get; set; }
        public string Message { get; set; }
    }
}
