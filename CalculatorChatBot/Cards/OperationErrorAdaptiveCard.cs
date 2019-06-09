// <copyright file="OperationErrorAdaptiveCard.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Cards
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Hosting;
    using CalculatorChatBot.Models;
    using CalculatorChatBot.Properties;

    public class OperationErrorAdaptiveCard
    {
        private static readonly string CardTemplate;

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