using PayerTracking.Library.Models;

namespace PayerTracking.Library.DataAccess
{
    public class MemoryDataAccess : IDataAccess
    {
        private readonly Dictionary<int, MemoryTransactionRecord> records = new();
        private int primaryKeyGenerator = 0;

        public void CreateNewPointTransaction(PointTransaction newTransaction)
        {
            var newTransactionId = primaryKeyGenerator++;
            records[newTransactionId] = new MemoryTransactionRecord(newTransactionId, newTransaction.PayerName, newTransaction.Qty, newTransaction.TransactionDateTime);
        }

        public List<PointTransaction> GetAllTransactions()
        {
            return records.Values.Select(mtr => new PointTransaction(mtr.TransactionId, mtr.PayerName, mtr.Amount, mtr.TimeAdded)).ToList();
        }

        private class MemoryTransactionRecord
        {
            public MemoryTransactionRecord(int transactionId, string payerName, int amount, DateTime timeAdded)
            {
                TransactionId = transactionId;
                PayerName = payerName;
                Amount = amount;
                TimeAdded = timeAdded;
            }

            public int TransactionId { get; set; }
            public string PayerName { get; set; }
            public int Amount { get; set; }
            public DateTime TimeAdded { get; set; }
        }
    }
}
