namespace Asp.Net_Core_WebApi.Models
{
    public class ApiResult
    {
        public ApiResult(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        public static ApiResult From(int statusCode, string message) => new ApiResult(statusCode, message);
    }
}
