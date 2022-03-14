using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayerTracking.Library.DataAccess;
using PayerTracking.Library.Models;
using System;
using System.Linq;

namespace PayerTracking.Test.IntegrationTests
{
    /// <summary>
    /// I chose to make this an "integration test" because normally this would be hitting a DB or an API as an actual extrnal integration.
    /// </summary>
    [TestClass]
    public class MemoryDataAccessTests
    {
        [TestMethod]
        public void CanReadAndWrite()
        {
            //Arrange
            var transaction = new PointTransaction("Dannon", 1000, DateTime.Now);
            var dataAccess = new MemoryDataAccess();

            //act
            dataAccess.CreateNewPointTransaction(transaction);
            var readResult = dataAccess.GetAllTransactions();

            //Assert
            Assert.AreEqual(1, readResult.Count);
            Assert.IsTrue(readResult.Any(pt => pt.PayerName == transaction.PayerName 
                && pt.Qty == transaction.Qty 
                && pt.TransactionDateTime == transaction.TransactionDateTime 
                && pt.TransactionId == 0));
        }

        [TestMethod]
        public void PrimaryKeyGeneratorIncrements()
        {
            //Arrange
            var transaction1 = new PointTransaction("Dannon", 1000, DateTime.Now);
            var transaction2 = new PointTransaction("Yoplait", 1000, DateTime.Now);
            var dataAccess = new MemoryDataAccess();

            //act
            dataAccess.CreateNewPointTransaction(transaction1);
            dataAccess.CreateNewPointTransaction(transaction2);
            var readResult = dataAccess.GetAllTransactions();

            //Assert
            Assert.AreEqual(2, readResult.Count);
            Assert.IsTrue(readResult.Any(pt => pt.PayerName == transaction1.PayerName 
                && pt.Qty == transaction1.Qty 
                && pt.TransactionDateTime == transaction1.TransactionDateTime 
                && pt.TransactionId == 0));
            Assert.IsTrue(readResult.Any(pt => pt.PayerName == transaction2.PayerName 
                && pt.Qty == transaction2.Qty 
                && pt.TransactionDateTime == transaction2.TransactionDateTime 
                && pt.TransactionId == 1));
        }
    }
}
