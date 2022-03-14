namespace PayerTracking.Library.Results
{
    public class HttpResult : Result
    {
        public HttpResult(int statusCode, string errorMsg = "") : base(errorMsg)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
    }

    public class HttpDataResult<T> : HttpResult
    {
        public HttpDataResult(T data, int statusCode, string errorMsg = "") : base(statusCode, errorMsg)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
