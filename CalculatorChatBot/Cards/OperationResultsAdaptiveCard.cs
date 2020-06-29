﻿// <copyright file="OperationResultsAdaptiveCard.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Cards
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Hosting;
    using CalculatorChatBot.Models;
    using CalculatorChatBot.Properties;

    public class OperationResultsAdaptiveCard
    {
        private static readonly string CardTemplate;

        static OperationResultsAdaptiveCard()
        {
            var cardJsonFilePath = HostingEnvironment.MapPath("~/Cards/OperationResultsAdaptiveCard.json");
            CardTemplate = File.ReadAllText(cardJsonFilePath);
        }

        public static string GetCard(OperationResults opsResults)
        {
            var resultsCardTitleText = Resources.ResultsCardTitleText;
            var operationTypeText = string.Format(Resources.OperationTypeText, opsResults.OperationType);
            var inputLineText = string.Format(Resources.InputLineText, opsResults.Input);
            var outputResultText = string.Format(Resources.OutputResultTypeText, opsResults.ResultType, opsResults.NumericalResult);

            var variablesToValues = new Dictionary<string, string>()
            {
                { "resultsCardTitleText", resultsCardTitleText },
                { "operationTypeText", operationTypeText },
                { "inputLineText", inputLineText },
                { "outputResultText", outputResultText }
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