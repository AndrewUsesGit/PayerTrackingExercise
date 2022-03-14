namespace PayerTracking.Library.Results
{
    public class Result
    {
        public Result(string errorMsg = "")
        {
            if (string.IsNullOrEmpty(errorMsg))
            {
                IsSuccessful = true;
                ErrorMessage = "";
            }
            else
            {
                IsSuccessful = false;
                ErrorMessage = errorMsg.Trim();
            }
        }

        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class DataResult<T> : Result
    {
        public DataResult(T data, string errorMsg = "") : base(errorMsg)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
