namespace PayerTracking.Library.Helpers
{
    public static class RequestValidator
    {
        public static string ValidateAddPointRequest(string payer, int qtyToAdd, DateTime timeStamp)
        {
            var errorMsg = "";

            if (string.IsNullOrEmpty(payer))
                errorMsg += "No payer specified. ";
            if (qtyToAdd == 0)
                errorMsg += "Quantity should be non zero. ";
            if (timeStamp == default)
                errorMsg += "TimeStamp is not valid. ";

            return errorMsg.Trim();
        }

        public static string ValidateSpendPointRequest(int qtyToSpend, int qtyAvailable)
        {
            if (qtyToSpend <= 0)
                return "Need to spend at least 1 point.";

            return qtyToSpend > qtyAvailable
                ? "Trying to spend more points than available."
                : "";
        }
    }
}
