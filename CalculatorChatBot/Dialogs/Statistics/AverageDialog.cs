// <copyright file="AverageDialog.cs" company="XYZ Software LLC">
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
    /// Given a list of integers, this dialog will return the average or mean.
    /// </summary>
    [Serializable]
    public class AverageDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AverageDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public AverageDialog(Activity incomingActivity)
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
                int sum = this.InputInts[0];
                for (int i = 1; i < this.InputInts.Length; i++)
                {
                    sum += this.InputInts[i];
                }

                decimal mean = Convert.ToDecimal(sum) / this.InputInts.Length;
                var successResType = ResultTypes.Average;

                var results = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(mean, 2).ToString(),
                    OutputMsg = $"Given the list: {this.InputString}; the average = {decimal.Round(mean, 2)}",
                    OperationType = operationType.GetDescription(),
                    ResultType = successResType.GetDescription()
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
                    OutputMsg = "Your list may be too small to calculate an average. Please try again later.",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorReplyAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorReplyAdaptiveCard)
                    }
                };
                await context.PostAsync(errorReply);
            }

            context.Done<object>(null);
        }
    }
}