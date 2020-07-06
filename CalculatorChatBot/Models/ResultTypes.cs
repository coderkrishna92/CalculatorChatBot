// <copyright file="ResultTypes.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Models
{
    using System.ComponentModel;

    /// <summary>
    /// This enumeration is the result types.
    /// </summary>
#pragma warning disable CA1717 // Only FlagsAttribute enums should have plural names
    public enum ResultTypes
#pragma warning restore CA1717 // Only FlagsAttribute enums should have plural names
    {
        /// <summary>
        /// This is the sum result type.
        /// </summary>
        [Description("Sum")]
        Sum,

        /// <summary>
        /// This is the difference result type.
        /// </summary>
        [Description("Difference")]
        Difference,

        /// <summary>
        /// This is the product result type.
        /// </summary>
        [Description("Product")]
        Product,

        /// <summary>
        /// This is the quotient result type.
        /// </summary>
        [Description("Quotient")]
        Quotient,

        /// <summary>
        /// This is the remainder result type.
        /// </summary>
        [Description("Remainder")]
        Remainder,

        /// <summary>
        /// This is the average result type.
        /// </summary>
        [Description("Average")]
        Average,

        /// <summary>
        /// This is the median result type.
        /// </summary>
        [Description("Median")]
        Median,

        /// <summary>
        /// This is the mode result type.
        /// </summary>
        [Description("Mode")]
        Mode,

        /// <summary>
        /// This is the range result type.
        /// </summary>
        [Description("Range")]
        Range,

        /// <summary>
        /// This is the hypotenuse result type.
        /// </summary>
        [Description("Hypotenuse")]
        Hypotenuse,

        /// <summary>
        /// This is the quadratic roots result type.
        /// </summary>
        [Description("Quadratic Roots")]
        EquationRoots,

        /// <summary>
        /// This is the error result type.
        /// </summary>
        [Description("Error")]
        Error,

        /// <summary>
        /// This is the discriminant result type.
        /// </summary>
        [Description("Discriminant")]
        Discriminant,

        /// <summary>
        /// This is the variance result type.
        /// </summary>
        [Description("Variance")]
        Variance,

        /// <summary>
        /// This is the geometric mean result type.
        /// </summary>
        [Description("Geometric Mean")]
        GeometricMean,

        /// <summary>
        /// This is the standard deviation result type.
        /// </summary>
        [Description("Standard Deviation")]
        StandardDeviation,

        /// <summary>
        /// This is the root mean square result type.
        /// </summary>
        [Description("Root Mean Square")]
        RootMeanSquare,

        /// <summary>
        /// This is the midpoint result type.
        /// </summary>
        [Description("Midpoint")]
        Midpoint,

        /// <summary>
        /// This is the distance result type.
        /// </summary>
        [Description("Distance")]
        Distance,

        /// <summary>
        /// This is the triangle area result type.
        /// </summary>
        [Description("Area of a Triangle")]
        TriangleArea,

        /// <summary>
        /// This is the square area result type.
        /// </summary>
        [Description("Area of a Square")]
        SquareArea,

        /// <summary>
        /// This is the rectangle area result type.
        /// </summary>
        [Description("Area of a Rectangle")]
        RectangleArea,

        /// <summary>
        /// This is the circle area result type.
        /// </summary>
        [Description("Area of a Circle")]
        CircleArea,

        /// <summary>
        /// This is the triangle perimeter result type.
        /// </summary>
        [Description("Triangle Perimeter")]
        TrianglePerimeter,

        /// <summary>
        /// This is the quadrilateral perimeter result type.
        /// </summary>
        [Description("Quadrilateral Perimeter")]
        QuadPerimeter,

        /// <summary>
        /// This is the square perimeter result type.
        /// </summary>
        [Description("Square Perimeter")]
        SquarePerimeter,

        /// <summary>
        /// This is the Circumference result type.
        /// </summary>
        [Description("Circumference")]
        Circumference,

        /// <summary>
        /// This is the TrapezoidArea result type.
        /// </summary>
        [Description("Area of a Trapezoid")]
        TrapezoidArea,
    }
}