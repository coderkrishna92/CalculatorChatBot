// <copyright file="Extensions.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Models
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// This is the extensions class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// This method to get the description.
        /// </summary>
        /// <typeparam name="T">A generic type.</typeparam>
        /// <param name="e">The enumeration.</param>
        /// <returns>A string value of the description.</returns>
        public static string GetDescription<T>(this T e)
            where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));

                        if (memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null;
        }
    }
}