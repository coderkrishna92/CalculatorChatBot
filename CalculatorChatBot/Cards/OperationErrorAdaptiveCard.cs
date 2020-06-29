// <copyright file="OperationErrorAdaptiveCard.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Cards
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Hosting;
    using CalculatorChatBot.Models;
    using CalculatorChatBot.Properties;

    /// <summary>
    /// This is the operation error adaptive card class.
    /// </summary>
    public static class OperationErrorAdaptiveCard
    {
        private static readonly string CardTemplate;

        /// <summary>
        /// Initializes static members of the <see cref="OperationErrorAdaptiveCard"/> class.
        /// </summary>
        static OperationErrorAdaptiveCard()
        {
            var cardJsonFilePath = HostingEnvironment.MapPath("~/Cards/OperationErrorAdaptiveCard.json");
            CardTemplate = File.ReadAllText(cardJsonFilePath);
        }

        public static string GetCard(OperationResults errorResults)
        {
            var errorCardTitleText = Resources.ErrorCardTitleText;
            var operationTypeText = string.Format(Resources.OperationTypeText, errorResults.OperationType);
            var inputLineText = string.Format(Resources.InputLineText, errorResults.Input);
            var outputResultText = string.Format(Resources.OutputResultTypeText, errorResults.ResultType, errorResults.NumericalResult);

            var variablesToValues = new Dictionary<string, string>()
            {
                { "errorCardTitleText", errorCardTitleText },
                { "operationTypeText", operationTypeText },
                { "inputLineText", inputLineText },
                { "outputResultText", outputResultText },
            };

            var cardBody = CardTemplate;
            foreach (var kvp in variablesToValues)
            {
                cardBody = cardBody.Replace($"%{kvp.Key}%", kvp.Value);
            }

            return cardBody;
        }
    }
}