using PayerTracking.Library.Models;
using PayerTracking.Library.Results;
using PayerTracking.Library.DataAccess;
using PayerTracking.Library.Helpers;
using System.Net;

namespace PayerTracking.Library
{
    public class Orchestrator
    {
        private readonly IDataAccess dataAccess;

        public Orchestrator(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public HttpResult AddPoints(string payerName, int qtyToAdd, DateTime timeStamp)
        {
            var validationError = RequestValidator.ValidateAddPointRequest(payerName, qtyToAdd, timeStamp);
            if (!string.IsNullOrEmpty(validationError))
            {
                return new HttpResult((int)HttpStatusCode.BadRequest, validationError);
            }

            dataAccess.CreateNewPointTransaction(new PointTransaction(payerName.ToUpper(), qtyToAdd, timeStamp));

            return new HttpResult((int)HttpStatusCode.OK);
        }

        public HttpDataResult<List<PointTransaction>> SpendPoints(int qtyToSpend)
        {
            //Load data and validate request
            var transactions = dataAccess.GetAllTransactions();

            var validationError = RequestValidator.ValidateSpendPointRequest(qtyToSpend, transactions.Sum(pt => pt.Qty));
            if (!string.IsNullOrEmpty(validationError))
            {
                return new HttpDataResult<List<PointTransaction>>(new List<PointTransaction>(), (int)HttpStatusCode.BadRequest, validationError);
            }

            //Do work
            var newTransactions = PointAccountant.CreateNewSpendTransactions(transactions, qtyToSpend);

            //Save changes and return
            foreach(var transaction in newTransactions)
            {
                dataAccess.CreateNewPointTransaction(transaction);
            }

            return new HttpDataResult<List<PointTransaction>>(newTransactions, (int)HttpStatusCode.OK);
        }

        public HttpDataResult<Dictionary<string, int>> GetBalances()
        {
            var transactions = dataAccess.GetAllTransactions();
            var payerToTotalMap = transactions.GroupBy(pt => pt.PayerName).ToDictionary(g => g.Key, g => g.Sum(pt => pt.Qty));

            return new HttpDataResult<Dictionary<string, int>>(payerToTotalMap, (int)HttpStatusCode.OK);
        }
    }
}
