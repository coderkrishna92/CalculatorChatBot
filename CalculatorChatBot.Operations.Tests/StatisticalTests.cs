// <copyright file="StatisticalTests.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Operations.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class contains the various unit tests for the <see cref="StatisticalOps"/> class.
    /// </summary>
    [TestClass]
    public class StatisticalTests
    {
        /// <summary>
        /// This method tests the functionality of calculating the mean.
        /// </summary>
        [TestMethod]
        public void AverageTest()
        {
            string inputStr = "1,2,3";
            decimal testMean = 2;

            var expectedMean = StatisticalOps.CalculateAverage(inputStr);
            Assert.AreEqual(testMean, expectedMean);

            if (testMean == expectedMean)
            {
                Console.Write($"Expected Mean - {expectedMean}, Test Mean - {testMean}" + Environment.NewLine);
                Console.Write("The AverageTest calculations pass");
            }
        }

        /// <summary>
        /// This method will be testing the functionality of calculating the average.
        /// </summary>
        [TestMethod]
        public void MedianTest()
        {
            string inputStr = "2,4,5,7,9";
            decimal testMedian = 5;

            var expectedMedian = StatisticalOps.CalculateMedian(inputStr);
            Assert.AreEqual(testMedian, expectedMedian);

            if (testMedian == expectedMedian)
            {
                Console.Write($"Expected Median - {expectedMedian}, Test Median - {testMedian}" + Environment.NewLine);
                Console.Write("The MedianTest calculations pass");
            }
        }

        /// <summary>
        /// This test method will test the functionality of calculating the mode.
        /// </summary>
        [TestMethod]
        public void ModeTest()
        {
            string inputStr = "1,1,2,3,5,5,7";
            int[] testModes = StatisticalOps.CalculateMode(inputStr);

            int[] expectedModes = new int[] { 1, 5 };

            if (testModes.Length == expectedModes.Length)
            {
                for (int i = 0; i < testModes.Length; i++)
                {
                    Assert.AreEqual(testModes[i], expectedModes[i]);
                }

                Console.Write("The ModeTest calculations pass");
            }
        }

        /// <summary>
        /// This method will test the range calculation.
        /// </summary>
        [TestMethod]
        public void RangeTest()
        {
            string inputStr = "-3, 4, 2, 1";
            int testRange = StatisticalOps.CalculateRange(inputStr);

            int expectedRange = 7;
            Assert.AreEqual(testRange, expectedRange);

            if (testRange == expectedRange)
            {
                Console.Write("The RangeTest calculations pass");
            }
        }

        /// <summary>
        /// This method tests the standard deviation functionality.
        /// </summary>
        [TestMethod]
        public void StandardDeviationTest()
        {
            string inputStr = "-3, 4, 2, 1";
            double expectedStandardDev = 2.55;

            double testStandardDev = StatisticalOps.CalculateStandardDeviation(inputStr);
            Assert.AreEqual(testStandardDev, expectedStandardDev);

            if (testStandardDev == expectedStandardDev)
            {
                Console.Write("The StandardDeviationTest calculations pass");
            }
        }

        /// <summary>
        /// This method tests the calculation of the variance.
        /// </summary>
        [TestMethod]
        public void VarianceTest()
        {
            string inputStr = "-3, 4, 2, 1";
            double expectedVariance = 8.67;

            double testVariance = StatisticalOps.CalculateVariance(inputStr);
            Assert.AreEqual(expectedVariance, testVariance);

            if (testVariance == expectedVariance)
            {
                Console.Write("The VarianceTest calculations pass");
            }
        }
    }
}