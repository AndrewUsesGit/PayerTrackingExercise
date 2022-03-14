using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayerTracking.Library;
using PayerTracking.Library.DataAccess;
using PayerTracking.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PayerTracking.Test.UnitTests
{
    [TestClass]
    public class OrchestratorTests
    {
        private IDataAccess dataAccess;
        private const int badRequestStatusCode = 400;
        private const int okStatusCode = 200;

        [TestInitialize]
        public void Init()
        {
            dataAccess = A.Fake<IDataAccess>();
        }

        [TestMethod]
        public void AddPoints_InvalidPayerName()
        {
            //Arrange
            var payerName = "";
            var qtyToAdd = 1000;
            var dateTime = DateTime.Now;

            //Act
            var result = new Orchestrator(dataAccess).AddPoints(payerName, qtyToAdd, dateTime);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(badRequestStatusCode, result.StatusCode);
            Assert.AreEqual("No payer specified.", result.ErrorMessage);
        }

        [TestMethod]
        public void AddPoints_InvalidQty()
        {
            //Arrange
            var payerName = "DANNON";
            var qtyToAdd = 0;
            var dateTime = DateTime.Now;

            //Act
            var result = new Orchestrator(dataAccess).AddPoints(payerName, qtyToAdd, dateTime);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(badRequestStatusCode, result.StatusCode);
            Assert.AreEqual("Quantity should be non zero.", result.ErrorMessage);
            A.CallTo(() => dataAccess.CreateNewPointTransaction(A<PointTransaction>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void AddPoints_InvalidTimeStamp()
        {
            //Arrange
            var payerName = "DANNON";
            var qtyToAdd = 1000;
            var dateTime = default(DateTime);

            //Act
            var result = new Orchestrator(dataAccess).AddPoints(payerName, qtyToAdd, dateTime);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(badRequestStatusCode, result.StatusCode);
            Assert.AreEqual("TimeStamp is not valid.", result.ErrorMessage);
            A.CallTo(() => dataAccess.CreateNewPointTransaction(A<PointTransaction>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void AddPoints_AllInvalid()
        {
            //Arrange
            var payerName = "";
            var qtyToAdd = 0;
            var dateTime = default(DateTime);

            //Act
            var result = new Orchestrator(dataAccess).AddPoints(payerName, qtyToAdd, dateTime);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(badRequestStatusCode, result.StatusCode);
            Assert.AreEqual("No payer specified. Quantity should be non zero. TimeStamp is not valid.", result.ErrorMessage);
            A.CallTo(() => dataAccess.CreateNewPointTransaction(A<PointTransaction>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void AddPoints_Good()
        {
            //Arrange
            var payerName = "DANNON";
            var qtyToAdd = 1000;
            var dateTime = DateTime.Now;

            //Act
            var result = new Orchestrator(dataAccess).AddPoints(payerName, qtyToAdd, dateTime);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(okStatusCode, result.StatusCode);
            Assert.AreEqual("", result.ErrorMessage);
            A.CallTo(() => dataAccess.CreateNewPointTransaction(A<PointTransaction>._)).MustHaveHappened();
        }

        [TestMethod]
        public void AddPoints_LowerCaseStoredAsUpper()
        {
            //Arrange
            const string lowerDannon = "dannon";
            var payerName = lowerDannon;
            var qtyToAdd = 1000;
            var dateTime = DateTime.Now;

            //Act
            var result = new Orchestrator(dataAccess).AddPoints(payerName, qtyToAdd, dateTime);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(okStatusCode, result.StatusCode);
            Assert.AreEqual("", result.ErrorMessage);
            A.CallTo(() => dataAccess.CreateNewPointTransaction(A<PointTransaction>.That.Matches(pt => pt.PayerName == lowerDannon.ToUpper()))).MustHaveHappened();
        }

        [TestMethod]
        public void SpendPoints_ZeroQty()
        {
            //Arrange
            var qtyToSpend = 0;

            //Act
            var result = new Orchestrator(dataAccess).SpendPoints(qtyToSpend);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(badRequestStatusCode, result.StatusCode);
            Assert.AreEqual("Need to spend at least 1 point.", result.ErrorMessage);
            A.CallTo(() => dataAccess.CreateNewPointTransaction(A<PointTransaction>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void SpendPoints_NegativeQty()
        {
            //Arrange
            var qtyToSpend = -10;

            //Act
            var result = new Orchestrator(dataAccess).SpendPoints(qtyToSpend);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(badRequestStatusCode, result.StatusCode);
            Assert.AreEqual("Need to spend at least 1 point.", result.ErrorMessage);
            A.CallTo(() => dataAccess.CreateNewPointTransaction(A<PointTransaction>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void SpendPoints_Overspend()
        {
            //Arrange
            var qtyToSpend = 100;
            A.CallTo(() => dataAccess.GetAllTransactions()).Returns(new List<PointTransaction>());

            //Act
            var result = new Orchestrator(dataAccess).SpendPoints(qtyToSpend);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(badRequestStatusCode, result.StatusCode);
            Assert.AreEqual("Trying to spend more points than available.", result.ErrorMessage);
            A.CallTo(() => dataAccess.CreateNewPointTransaction(A<PointTransaction>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void SpendPoints_Good()
        {
            //Arrange
            var qtyToSpend = 200;
            A.CallTo(() => dataAccess.GetAllTransactions()).Returns(new List<PointTransaction>
            {
                new("DANNON", 100, DateTime.Now),
                new("YOPLAIT", 100, DateTime.Now)
            });

            //Act
            var result = new Orchestrator(dataAccess).SpendPoints(qtyToSpend);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(okStatusCode, result.StatusCode);
            Assert.AreEqual("", result.ErrorMessage);
            A.CallTo(() => dataAccess.CreateNewPointTransaction(A<PointTransaction>._)).MustHaveHappened();
        }

        [TestMethod]
        public void GetBalances_NoTransactions()
        {
            //Arrange
            A.CallTo(() => dataAccess.GetAllTransactions()).Returns(new List<PointTransaction>());

            //Act
            var result = new Orchestrator(dataAccess).GetBalances();

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsFalse(result.Data.Any());
        }

        [TestMethod]
        public void GetBalances_WithTransactions()
        {
            //Arrange
            A.CallTo(() => dataAccess.GetAllTransactions()).Returns(new List<PointTransaction>
            {
                new("DANNON", 100, DateTime.Now),
                new("YOPLAIT", 100, DateTime.Now),
                new("YOPLAIT", 100, DateTime.Now),
            });
            //Act
            var result = new Orchestrator(dataAccess).GetBalances();

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(result.Data.Count == 2);
            Assert.AreEqual(100, result.Data["DANNON"]);
            Assert.AreEqual(200, result.Data["YOPLAIT"]);
        }
    }
}
