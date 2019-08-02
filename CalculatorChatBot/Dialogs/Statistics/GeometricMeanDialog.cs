// <copyright file="GeometricMeanDialog.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;

    /// <summary>
    /// Given a list of integers, this dialog calculates the geometric mean of the list.
    /// </summary>
    [Serializable]
    public class GeometricMeanDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricMeanDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public GeometricMeanDialog(Activity incomingActivity)
        {
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            if (!string.IsNullOrEmpty(incomingInfo[1]))
            {
                this.InputString = incomingInfo[1];
                this.InputStringArray = this.InputString.Split(',');
                this.InputInts = Array.ConvertAll(this.InputStringArray, int.Parse);
            }
        }

        public string InputString { get; set; }

        public string[] InputStringArray { get; set; }

        public int[] InputInts { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var operationType = CalculationTypes.Statistical;
            if (this.InputInts.Length > 1)
            {
                int product = this.InputInts[0];
                for (int i = 1; i < this.InputInts.Length; i++)
                {
                    if (this.InputInts[i] != 0)
                    {
                        product *= this.InputInts[i];
                    }
                    else
                    {
                        break;
                    }
                }

                // Calculating the Geometric mean here
                decimal geometricMean = Convert.ToDecimal(Math.Pow(product, 1 / this.InputInts.Length));
                var resultType = ResultTypes.GeometricMean;

                var results = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(geometricMean, 2).ToString(),
                    OutputMsg = $"Given the list: {this.InputString}; the geometric mean = ${geometricMean.ToString()}",
                    OperationType = operationType.GetDescription(),
                    ResultType = resultType.GetDescription()
                };

                IMessageActivity opsReply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(results);
                opsReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard)
                    }
                };

                await context.PostAsync(opsReply);
            }
            else
            {
                var errorResType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = "Your list may be too small to calculate the geometric mean. Please try again later",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorOpsAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorOpsAdaptiveCard)
                    }
                };
                await context.PostAsync(errorReply);
            }

            // Returning back to the RootDialog
            context.Done<object>(null);
        }
    }
}