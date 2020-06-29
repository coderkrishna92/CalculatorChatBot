// <copyright file="CalculationTypes.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Models
{
    using System.ComponentModel;

    /// <summary>
    /// This enumeration details the calcuation types.
    /// </summary>
#pragma warning disable CA1717 // Only FlagsAttribute enums should have plural names
    public enum CalculationTypes
#pragma warning restore CA1717 // Only FlagsAttribute enums should have plural names
    {
        /// <summary>
        /// The arithmetic calculation type.
        /// </summary>
        [Description("Arithmetic")]
        Arithmetic,

        /// <summary>
        /// The statistical calculation type.
        /// </summary>
        [Description("Statistical")]
        Statistical,

        /// <summary>
        /// The geometric calculation type.
        /// </summary>
        [Description("Geometric")]
        Geometric,
    }
}