namespace PayerTracking.Api.DTOs.Requests
{
    public class AddPointsRequest
    {
        public string PayerName { get; set; } = "";
        public int Amount { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
