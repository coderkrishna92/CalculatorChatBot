// <copyright file="ArithmeticTests.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Operations.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// This class contains the unit tests of the ArithmeticOps.cs class.
    /// </summary>
    [TestClass]
    public class ArithmeticTests
    {
        private readonly Mock<IArithmeticOps> mockOps = new Mock<IArithmeticOps>();

        /// <summary>
        /// This is the test method for testing the overall summation.
        /// </summary>
        [TestMethod]
        public void OverallSummationTest()
        {
            var inputString = "1,3,4";

            var expSum = this.mockOps.Object.OverallSum(inputString);
            int testSum = 8;

            Assert.AreEqual(expSum, testSum);

            if (testSum == expSum)
            {
                Console.Write($"Expected overall sum: {expSum}; Test overall sum: {testSum}" + Environment.NewLine);
                Console.Write("The OverallSummation test has passed");
            }
        }

        /// <summary>
        /// This method is testing the overall difference.
        /// </summary>
        [TestMethod]
        public void OverallDifferenceTest()
        {
            var inputString = "-1,0,-3";

            var expOverallDiff = this.mockOps.Object.OverallDifference(inputString);
            int testOverallDiff = 2;

            Assert.AreEqual(expOverallDiff, testOverallDiff);

            if (testOverallDiff == expOverallDiff)
            {
                Console.Write($"Expected overall difference: {expOverallDiff}; Test overall difference: {testOverallDiff}" + Environment.NewLine);
                Console.Write("The OverallDifference test has passed");
            }
        }

        /// <summary>
        /// This method is a test for finding the overall product.
        /// </summary>
        [TestMethod]
        public void OverallProductTest()
        {
            var inputString = "1,0,4";

            var expOverallProd1 = this.mockOps.Object.OverallProduct(inputString);
            int testOverallProd1 = 0;

            Assert.AreEqual(expOverallProd1, testOverallProd1);

            var scenario = testOverallProd1 == expOverallProd1;
            if (scenario)
            {
                Console.Write($"Scenario - Expected overall product: {expOverallProd1}; Test overall product: {testOverallProd1}" + Environment.NewLine);
                Console.Write("The OverallProduct test has passed");
            }
        }

        /// <summary>
        /// This method also tests for the overall product.
        /// </summary>
        [TestMethod]
        public void OverallProductTest2()
        {
            var inputString2 = "1,3,-2";
            var expOverallProd2 = this.mockOps.Object.OverallProduct(inputString2);
            int testOverallProd2 = -6;

            Assert.AreEqual(expOverallProd2, testOverallProd2);

            if (testOverallProd2 == expOverallProd2)
            {
                Console.Write($"Scenario - Expected overall product: {expOverallProd2}; Test overall product: {testOverallProd2}" + Environment.NewLine);
                Console.Write("The OverallProduct test has passed");
            }
        }

        /// <summary>
        /// This method tests the functionality to calculate the quotient.
        /// </summary>
        [TestMethod]
        public void OverallDivisionTest()
        {
            var inputString = "2,10";

            var expectedQuotient = this.mockOps.Object.OverallDivision(inputString);
            decimal testQuotient = 0.2m;

            Assert.AreEqual(expectedQuotient, testQuotient);

            if (testQuotient == expectedQuotient)
            {
                Console.Write($"Expected overall quotient: {expectedQuotient}; Test overall difference: {testQuotient}" + Environment.NewLine);
                Console.Write("The OverallDivision test has passed");
            }
        }

        /// <summary>
        /// This method tests the calculation of the remainder.
        /// </summary>
        [TestMethod]
        public void OverallModuloTest()
        {
            var inputString = "2,10";
            var expectedResult = this.mockOps.Object.OverallModulo(inputString);
            var testResult = 2;

            Assert.AreEqual(expectedResult, testResult);

            if (expectedResult == testResult)
            {
                Console.Write($"Expected overall remainder: {expectedResult}; Test overall remainder: {testResult}" + Environment.NewLine);
                Console.Write("The OverallModulo test has passed");
            }
        }
    }
}