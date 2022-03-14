using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayerTracking.Library.Helpers;
using PayerTracking.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PayerTracking.Test.UnitTests
{
    [TestClass]
    public class PointAccountantTests
    {
        [TestMethod]
        public void CreateTransactions_NullTransactions()
        {
            //Arrange
            const int qtyToSpend = 100;

            //Act
            var result = PointAccountant.CreateNewSpendTransactions(null, qtyToSpend);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }
        
        [TestMethod]
        public void CreateTransactions_EmptyTransactions()
        {
            //Arrange
            const int qtyToSpend = 100;

            //Act
            var result = PointAccountant.CreateNewSpendTransactions(new List<PointTransaction>(), qtyToSpend);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }
        
        [TestMethod]
        public void CreateTransactions_SpendZero()
        {
            //Arrange
            const int qtyToSpend = 0;
            var transactions = new List<PointTransaction>
            {
                new("DANNON", 1000, DateTime.Parse("2020-11-02T14:00:00Z"))
            };

            //Act
            var result = PointAccountant.CreateNewSpendTransactions(transactions, qtyToSpend);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }
        
        [TestMethod]
        public void CreateTransactions_SimpleGood()
        {
            //Arrange
            const int qtyToSpend = 100;
            var transactions = new List<PointTransaction>
            {
                new("DANNON", 1000, DateTime.Parse("2020-11-02T14:00:00Z"))
            };

            //Act
            var result = PointAccountant.CreateNewSpendTransactions(transactions, qtyToSpend);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Any(t => t.PayerName == "DANNON" && t.Qty == -qtyToSpend));
        }
        
        [TestMethod]
        public void CreateTransactions_ComplexGood()
        {
            //Arrange
            const int qtyToSpend = 5000;
            var transactions = new List<PointTransaction>
            {
                new("DANNON", 1000, DateTime.Parse("2020-11-02T14:00:00Z")),
                new("UNILEVER", 200, DateTime.Parse("2020-10-31T11:00:00Z")),
                new("DANNON", -200, DateTime.Parse("2020-10-31T15:00:00Z")),
                new("MILLER COORS", 10000, DateTime.Parse("2020-11-01T14:00:00Z")),
                new("DANNON", 300, DateTime.Parse( "2020-10-31T10:00:00Z"))
            };

            //Act
            var result = PointAccountant.CreateNewSpendTransactions(transactions, qtyToSpend);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 3);
            Assert.IsTrue(result.Any(t => t.PayerName == "DANNON" && t.Qty == -100));
            Assert.IsTrue(result.Any(t => t.PayerName == "UNILEVER" && t.Qty == -200));
            Assert.IsTrue(result.Any(t => t.PayerName == "MILLER COORS" && t.Qty == -4700));
        }
        
        [TestMethod]
        public void CreateTransactions_OverspendWithoutBlowingUp()
        {
            //Arrange
            const int qtyToSpend = 1000000;
            var transactions = new List<PointTransaction>
            {
                new("DANNON", 1000, DateTime.Parse("2020-11-02T14:00:00Z")),
                new("UNILEVER", 200, DateTime.Parse("2020-10-31T11:00:00Z")),
                new("DANNON", -200, DateTime.Parse("2020-10-31T15:00:00Z")),
                new("MILLER COORS", 10000, DateTime.Parse("2020-11-01T14:00:00Z")),
                new("DANNON", 300, DateTime.Parse( "2020-10-31T10:00:00Z"))
            };

            //Act
            var result = PointAccountant.CreateNewSpendTransactions(transactions, qtyToSpend);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 3);
            Assert.IsTrue(result.Any(t => t.PayerName == "DANNON" && t.Qty == -1100));
            Assert.IsTrue(result.Any(t => t.PayerName == "UNILEVER" && t.Qty == -200));
            Assert.IsTrue(result.Any(t => t.PayerName == "MILLER COORS" && t.Qty == -10000));
        }
    }
}
