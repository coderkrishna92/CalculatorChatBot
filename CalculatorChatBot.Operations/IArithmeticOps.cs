// <copyright file="IArithmeticOps.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Operations
{
    /// <summary>
    /// This method will implement all of the arithmetic functions.
    /// </summary>
    public interface IArithmeticOps
    {
        /// <summary>
        /// This method definition returns the overall sum.
        /// </summary>
        /// <param name="inputList">The list of integers.</param>
        /// <returns>The sum.</returns>
        int OverallSum(string inputList);

        /// <summary>
        /// This method definition returns the overall difference.
        /// </summary>
        /// <param name="inputList">The list of integers.</param>
        /// <returns>The difference.</returns>
        int OverallDifference(string inputList);

        /// <summary>
        /// This method definition returns the overall product.
        /// </summary>
        /// <param name="inputList">The list of integers.</param>
        /// <returns>The product of the list of integers.</returns>
        int OverallProduct(string inputList);

        /// <summary>
        /// This method definition returns the overall modulo result.
        /// </summary>
        /// <param name="inputList">The list of integers.</param>
        /// <returns>The overall remainder.</returns>
        int OverallModulo(string inputList);

        /// <summary>
        /// This method definition returns the overall quotient.
        /// </summary>
        /// <param name="inputList">The list of integers.</param>
        /// <returns>The overall quotient.</returns>
        decimal OverallDivision(string inputList);
    }
}