namespace PayerTracking.Library.Models
{
    public class PointTransaction
    {
        public PointTransaction(string payerName, int qty, DateTime timeCredited)
        {
            PayerName = payerName;
            Qty = qty;
            TransactionDateTime = timeCredited;
        }

        public PointTransaction(int transactionId, string payerName, int qty, DateTime timeCredited) : this(payerName, qty, timeCredited)
        {
            TransactionId = transactionId;
        }

        public int TransactionId { get; set; }
        public string PayerName { get; set; }
        public int Qty { get; set; }
        public DateTime TransactionDateTime { get; set; }

        public PointTransaction Clone()
        {
            return new PointTransaction(TransactionId, PayerName, Qty, TransactionDateTime);
        }
    }
}
