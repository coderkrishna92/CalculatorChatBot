// <copyright file="OperationResults.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Models
{
    /// <summary>
    /// This class defines the operation results.
    /// </summary>
    public class OperationResults
    {
        /// <summary>
        /// Gets or sets the operation type.
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// Gets or sets the input string.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets the numerical result.
        /// </summary>
        public string NumericalResult { get; set; }

        /// <summary>
        /// Gets or sets the output message.
        /// </summary>
        public string OutputMsg { get; set; }

        /// <summary>
        /// Gets or sets the result type.
        /// </summary>
        public string ResultType { get; set; }
    }
}