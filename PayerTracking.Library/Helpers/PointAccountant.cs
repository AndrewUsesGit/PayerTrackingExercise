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
            
            // tally up how many points have already been spent for each payer ever.
            var unaccountedSpentPointsPerPayer = new Dictionary<string, int>();
            foreach (var payerTransactionGroup in mutableTransactions.GroupBy(t => t.PayerName))
            {
                unaccountedSpentPointsPerPayer[payerTransactionGroup.Key] = payerTransactionGroup.Where(pt => pt.Qty < 0).Sum(pt => Math.Abs(pt.Qty));
            }

            var transactionDateTime = DateTime.Now;
            var pointsToSpendPerPayer = new Dictionary<string, PointTransaction>();
            foreach (var transaction in mutableTransactions.Where(pt => pt.Qty > 0).OrderBy(pt => pt.TransactionDateTime))
            {
                // Make sure we account for points that have been spent previously first.
                // if not in the dictionary, out var defaults to default, for int = 0
                if (!unaccountedSpentPointsPerPayer.TryGetValue(transaction.PayerName, out var unaccountedForSpentPoints))
                {
                    unaccountedSpentPointsPerPayer[transaction.PayerName] = unaccountedForSpentPoints;
                }

                var amountToSpendOnOldSpentPoints = Math.Min(unaccountedForSpentPoints, transaction.Qty);

                // if we don't have old spent points left to account for, we'll be "-= 0" a lot
                unaccountedSpentPointsPerPayer[transaction.PayerName] -= amountToSpendOnOldSpentPoints;
                transaction.Qty -= amountToSpendOnOldSpentPoints;

                if (transaction.Qty <= 0)
                {
                    continue;
                }

                // then spend new points once we get to uspent points for the payer.
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
