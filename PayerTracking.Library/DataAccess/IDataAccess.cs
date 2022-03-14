using PayerTracking.Library.Models;

namespace PayerTracking.Library.DataAccess
{
    public interface IDataAccess
    {
        public void CreateNewPointTransaction(PointTransaction transactionAmount);
        public List<PointTransaction> GetAllTransactions();
    }
}
