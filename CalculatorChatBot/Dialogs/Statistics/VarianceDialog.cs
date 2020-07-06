// <copyright file="VarianceDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;

    /// <summary>
    /// Given a list of integers, this dialog will calculate the variance of the list.
    /// </summary>
    [Serializable]
    public class VarianceDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VarianceDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public VarianceDialog(Activity incomingActivity)
        {
            if (incomingActivity is null)
            {
                throw new ArgumentNullException(nameof(incomingActivity));
            }

            string[] incomingInfo = incomingActivity.Text.Split(' ');

            if (!string.IsNullOrEmpty(incomingInfo[1]))
            {
                this.InputString = incomingInfo[1];
                this.InputStringArray = this.InputString.Split(',');
                this.InputInts = Array.ConvertAll(this.InputStringArray, int.Parse);
            }
        }

        /// <summary>
        /// Gets or sets the input string.
        /// </summary>
        public string InputString { get; set; }

        /// <summary>
        /// Gets or sets the input string array.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public string[] InputStringArray { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// Gets or sets the input integers.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public int[] InputInts { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// This method will execute when this dialog is being run at runtime.
        /// </summary>
        /// <param name="context">The current dialog to run.</param>
        /// <returns>A unit of execution.</returns>
        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var operationType = CalculationTypes.Statistical;
            if (this.InputInts.Length > 1)
            {
                int sum = this.InputInts[0];
                foreach (int item in this.InputInts)
                {
                    sum += item;
                }

                double mean = Convert.ToDouble(sum) / this.InputInts.Length;
                decimal variance = CalculateVariance(mean, this.InputInts);
                var successResultType = ResultTypes.Variance;

                var results = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(variance, 2).ToString(CultureInfo.InvariantCulture),
                    OutputMsg = $"Given the list: {this.InputString}; the variance = {decimal.Round(variance, 2)}",
                    OperationType = operationType.GetDescription(),
                    ResultType = successResultType.GetDescription(),
                };

                IMessageActivity resultsReply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(results);
                resultsReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard),
                    },
                };

                await context.PostAsync(resultsReply).ConfigureAwait(false);
            }
            else
            {
                var errorResType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = "Your list may be too small to calculate the variance. Please try again later.",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResType.GetDescription(),
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorReplyAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorReplyAdaptiveCard),
                    },
                };

                await context.PostAsync(errorReply).ConfigureAwait(false);
            }

            context.Done<object>(null);
        }

        /// <summary>
        /// This is the method that would calculate the variance among a list
        /// of numbers.
        /// </summary>
        /// <param name="mean">The average of the list of numbers.</param>
        /// <param name="inputInts">The input list of integers.</param>
        /// <returns>A decimal value that represents the variance.</returns>
        private static decimal CalculateVariance(double mean, int[] inputInts)
        {
            double squareDiffs = 0;
            int n = inputInts.Length;

            for (int i = 0; i < inputInts.Length; i++)
            {
                squareDiffs += Math.Pow(Math.Abs(Convert.ToDouble(inputInts[i]) - mean), 2);
            }

            return Convert.ToDecimal(squareDiffs / n);
        }
    }
}