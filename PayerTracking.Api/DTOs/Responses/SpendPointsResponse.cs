using PayerTracking.Library.Models;

namespace PayerTracking.Api.DTOs.Responses
{
    public class SpendPointsResponse
    {
        public SpendPointsResponse(PointTransaction transaction)
        {
            Payer = transaction.PayerName;
            Points = transaction.Qty;
        }

        public string Payer { get; set; }
        public int Points { get; set; }
    }
}
