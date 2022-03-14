using PayerTracking.Library.Models;

namespace PayerTracking.Library.Helpers
{
    public static class PointAccountant
    {
        public static List<PointTransaction> CreateNewSpendTransactions(IEnumerable<PointTransaction> transactions, int qtyToSpend)
        {
            if (transactions == null || !transactions.Any() || qtyToSpend <= 0)
                return new List<PointTransaction>();

            var qtyLeftToSpend = qtyToSpend;
            var mutableTransactions = new List<PointTransaction>(transactions.Select(t => t.Clone()));
            var unaccountedSpentPointsPerPayer = new Dictionary<string, int>();

            foreach (var payerTransactionGroup in mutableTransactions.GroupBy(t => t.PayerName))
            {
                unaccountedSpentPointsPerPayer[payerTransactionGroup.Key] = payerTransactionGroup.Where(pt => pt.Qty < 0).Sum(pt => Math.Abs(pt.Qty));
            }

            var transactionDateTime = DateTime.Now;
            var pointsToSpendPerPayer = new Dictionary<string, PointTransaction>();
            foreach (var transaction in mutableTransactions.Where(pt => pt.Qty > 0).OrderBy(pt => pt.TransactionDateTime))
            {
                // if not in the dictionary, out var defaults to default, for int = 0
                if (!unaccountedSpentPointsPerPayer.TryGetValue(transaction.PayerName, out var unaccountedForSpentPoints))
                {
                    unaccountedSpentPointsPerPayer[transaction.PayerName] = unaccountedForSpentPoints;
                }

                var amountToSpendOnOldSpentPoints = Math.Min(unaccountedForSpentPoints, transaction.Qty);

                unaccountedSpentPointsPerPayer[transaction.PayerName] -= amountToSpendOnOldSpentPoints;
                transaction.Qty -= amountToSpendOnOldSpentPoints;

                if (transaction.Qty <= 0)
                {
                    continue;
                }

                var amountToSpendOnNewSpentPoints = Math.Min(qtyLeftToSpend, transaction.Qty);

                if (!pointsToSpendPerPayer.TryGetValue(transaction.PayerName, out var newTransaction))
                {
                    newTransaction = new PointTransaction(transaction.PayerName, 0, transactionDateTime);
                    pointsToSpendPerPayer[transaction.PayerName] = newTransaction;
                }

                qtyLeftToSpend -= amountToSpendOnNewSpentPoints;
                newTransaction.Qty -= amountToSpendOnNewSpentPoints;

                if (qtyLeftToSpend <= 0)
                {
                    break;
                }
            }

            return pointsToSpendPerPayer.Values.ToList();
        }
    }
}
