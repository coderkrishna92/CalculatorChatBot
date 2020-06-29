// <copyright file="OperationResultsAdaptiveCard.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web.Hosting;
    using CalculatorChatBot.Models;
    using CalculatorChatBot.Properties;

    /// <summary>
    /// This is the adaptive card to render successful results of operations.
    /// </summary>
    public static class OperationResultsAdaptiveCard
    {
        private static readonly string CardTemplate;

        /// <summary>
        /// Initializes static members of the <see cref="OperationResultsAdaptiveCard"/> class.
        /// </summary>
        static OperationResultsAdaptiveCard()
        {
            var cardJsonFilePath = HostingEnvironment.MapPath("~/Cards/OperationResultsAdaptiveCard.json");
            CardTemplate = File.ReadAllText(cardJsonFilePath);
        }

        /// <summary>
        /// This method will construct the JSON string for the adaptive card.
        /// </summary>
        /// <param name="opsResults">The successful results object.</param>
        /// <returns>The JSON string.</returns>
        public static string GetCard(OperationResults opsResults)
        {
            if (opsResults is null)
            {
                throw new ArgumentNullException(nameof(opsResults));
            }

            var resultsCardTitleText = Resources.ResultsCardTitleText;
            var operationTypeText = string.Format(CultureInfo.InvariantCulture, Resources.OperationTypeText, opsResults.OperationType);
            var inputLineText = string.Format(CultureInfo.InvariantCulture, Resources.InputLineText, opsResults.Input);
            var outputResultText = string.Format(CultureInfo.InvariantCulture, Resources.OutputResultTypeText, opsResults.ResultType, opsResults.NumericalResult);

            var variablesToValues = new Dictionary<string, string>()
            {
                { "resultsCardTitleText", resultsCardTitleText },
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