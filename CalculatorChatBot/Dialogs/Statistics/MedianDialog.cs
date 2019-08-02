// <copyright file="MedianDialog.cs" company="XYZ Software LLC">
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
    /// Given a list of integers, this dialog will calculate the median (or the middle) of the list.
    /// </summary>
    [Serializable]
    public class MedianDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MedianDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public MedianDialog(Activity incomingActivity)
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

            if (this.InputInts.Length > 2)
            {
                decimal median;
                int size = this.InputInts.Length;
                int[] copyArr = this.InputInts;

                // Sorting the array
                Array.Sort(copyArr);

                if (size % 2 == 0)
                {
                    median = Convert.ToDecimal(copyArr[(size / 2) - 1] + copyArr[size / 2]) / 2;
                }
                else
                {
                    median = Convert.ToDecimal(copyArr[(size - 1) / 2]);
                }

                var opsResultType = ResultTypes.Median;
                var opsResult = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(median, 2).ToString(),
                    OutputMsg = $"Given the list: {this.InputString}; the median = {decimal.Round(median, 2)}",
                    OperationType = operationType.GetDescription(),
                    ResultType = opsResultType.GetDescription()
                };

                IMessageActivity opsReply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(opsResult);
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
                var errorResult = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"Please double check the input: {this.InputString} and try again",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorReplyAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResult);
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