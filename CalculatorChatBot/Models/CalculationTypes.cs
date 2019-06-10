// <copyright file="CalculationTypes.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Models
{
    using System.ComponentModel;

    public enum CalculationTypes
    {
        [Description("Arithmetic")]
        Arithmetic,
        [Description("Statistical")]
        Statistical,
        [Description("Geometric")]
        Geometric
    }
}