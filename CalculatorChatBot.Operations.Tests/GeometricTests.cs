// <copyright file="GeometricTests.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Operations.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class represents all the testing to be done for the <see cref="GeometricOps"/> class.
    /// </summary>
    [TestClass]
    public class GeometricTests
    {
        /// <summary>
        /// This method will test a condition for the discriminant where there are 2 imaginary roots.
        /// </summary>
        [TestMethod]
        public void ImaginaryRootsDiscriminantTest()
        {
            var inputString = "1, 1, 1";
            var actualDiscriminant = -3;

            var expectedDiscriminant = GeometricOps.CalculateDiscriminant(inputString);
            if (actualDiscriminant == expectedDiscriminant && expectedDiscriminant < 0)
            {
                Console.WriteLine("The imaginary roots discriminant test passes.");
            }
        }

        /// <summary>
        /// This method will test the condition for the discriminant where there is one root.
        /// </summary>
        [TestMethod]
        public void OneRootDiscriminantTest()
        {
            var inputString = "1, 2, 1";
            var actualDiscriminant = 0;

            var expectedDiscriminant = GeometricOps.CalculateDiscriminant(inputString);
            if (actualDiscriminant == expectedDiscriminant && expectedDiscriminant == 0)
            {
                Console.WriteLine("This test passes, and there is 1 root.");
            }
        }

        /// <summary>
        /// This method will test the condition for the discriminant where there are 2 real roots.
        /// </summary>
        [TestMethod]
        public void TwoRootsDiscriminantGreaterThanZero()
        {
            var inputString = "1,-5, 6";
            var actualDiscriminant = 1;

            var expectedDiscriminant = GeometricOps.CalculateDiscriminant(inputString);
            if (actualDiscriminant == expectedDiscriminant && expectedDiscriminant > 0)
            {
                Console.WriteLine("This test passes, and there are 2 real roots");
            }
        }
    }
}