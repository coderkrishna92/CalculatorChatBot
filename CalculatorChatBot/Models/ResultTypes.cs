// <copyright file="ResultTypes.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Models
{
    using System.ComponentModel;

    public enum ResultTypes
    {
        [Description("Sum")]
        Sum,
        [Description("Difference")]
        Difference,
        [Description("Product")]
        Product,
        [Description("Quotient")]
        Quotient,
        [Description("Remainder")]
        Remainder,
        [Description("Average")]
        Average,
        [Description("Median")]
        Median,
        [Description("Mode")]
        Mode,
        [Description("Range")]
        Range,
        [Description("Hypotenuse")]
        Hypotenuse,
        [Description("Quadratic Roots")]
        EquationRoots,
        [Description("Error")]
        Error,
        [Description("Discriminant")]
        Discriminant,
        [Description("Variance")]
        Variance,
        [Description("Geometric Mean")]
        GeometricMean,
        [Description("Standard Deviation")]
        StandardDeviation,
        [Description("Root Mean Square")]
        RootMeanSquare,
        [Description("Midpoint")]
        Midpoint,
        [Description("Distance")]
        Distance,
        [Description("Area of a Triangle")]
        TriangleArea,
        [Description("Area of a Square")]
        SquareArea,
        [Description("Area of a Rectangle")]
        RectangleArea,
        [Description("Area of a Circle")]
        CircleArea,
        [Description("Triangle Perimeter")]
        TrianglePerimeter,
        [Description("Quadrilateral Perimeter")]
        QuadPerimeter,
        [Description("Square Perimeter")]
        SquarePerimeter,
        [Description("Circumference")]
        Circumference,
        [Description("Area of a Trapezoid")]
        TrapezoidArea
    }
}