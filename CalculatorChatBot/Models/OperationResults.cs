// <copyright file="OperationResults.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Models
{
    public class OperationResults
    {
        public string OperationType { get; set; }

        public string Input { get; set; }

        public string NumericalResult { get; set; }

        public string OutputMsg { get; set; }

        public string ResultType { get; set; }
    }
}