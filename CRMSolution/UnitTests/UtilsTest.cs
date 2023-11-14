using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRMUtilities;

namespace UnitTests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void TestMonthsInDates_01()
        {
            // Arrange
            DateTime start = new DateTime(2016, 12, 25);
            DateTime end = new DateTime(2017, 10, 29);

            // Act
            double result = Utils.MonthsInDates(start, end);

            // Assert
            Assert.AreEqual(11, result);
        }

        [TestMethod]
        public void TestMonthsInDates_02()
        {
            // Arrange
            DateTime start = new DateTime(2017, 10, 29);
            DateTime end = new DateTime(2017, 10, 29);

            // Act
            double result = Utils.MonthsInDates(start, end);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestMonthsInDates_03()
        {
            // Arrange
            DateTime start = new DateTime(2017, 10, 19);
            DateTime end = new DateTime(2017, 10, 29);

            // Act
            double result = Utils.MonthsInDates(start, end);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestMonthsInDates_04()
        {
            // Arrange
            DateTime start = new DateTime(2016, 01, 19);
            DateTime end = new DateTime(2017, 10, 29);

            // Act
            double result = Utils.MonthsInDates(start, end);

            // Assert
            Assert.AreEqual(22, result);
        }

        [TestMethod]
        public void TestMonthsInDates_05()
        {
            // Arrange
            DateTime start = new DateTime(2017, 11, 19);
            DateTime end = new DateTime(2017, 10, 29);

            // Act
            double result = Utils.MonthsInDates(start, end);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMonthsInDates_06()
        {
            // Arrange
            DateTime start = new DateTime(2017, 12, 19);
            DateTime end = new DateTime(2017, 10, 29);

            // Act
            double result = Utils.MonthsInDates(start, end);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMonthsInDates_07()
        {
            // Arrange
            DateTime start = new DateTime(2016, 10, 19);
            DateTime end = new DateTime(2017, 10, 29);

            // Act
            double result = Utils.MonthsInDates(start, end);

            // Assert
            Assert.AreEqual(13, result);
        }
    }
}
