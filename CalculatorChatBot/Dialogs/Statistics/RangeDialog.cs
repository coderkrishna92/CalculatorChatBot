// <copyright file="RangeDialog.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;

    /// <summary>
    /// Given a list of integers, this dialog will calculate the range of the list.
    /// </summary>
    [Serializable]
    public class RangeDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public RangeDialog(Activity incomingActivity)
        {
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            if (!string.IsNullOrEmpty(incomingInfo[1]))
            {
                this.InputString = incomingInfo[1];
                this.InputStringArray = this.InputString.Split(',');
                this.InputInts = Array.ConvertAll(this.InputStringArray, int.Parse);
            }
        }

        /// <summary>
        /// Gets or sets the InputString.
        /// </summary>
        public string InputString { get; set; }

        /// <summary>
        /// Gets or sets the InputStringArray.
        /// </summary>
        public string[] InputStringArray { get; set; }

        /// <summary>
        /// Gets or sets the InputInts array.
        /// </summary>
        public int[] InputInts { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var operationType = CalculationTypes.Statistical;

            if (this.InputInts.Length >= 2)
            {
                var inputIntMax = this.InputInts.Max();

                var inputIntMin = this.InputInts.Min();

                // Conduct the range calculation as max - min
                var range = inputIntMax - inputIntMin;

                var successResType = ResultTypes.Range;
                var successResult = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = range.ToString(),
                    OutputMsg = $"Given the list: {this.InputString}; the range = {range}",
                    OperationType = operationType.GetDescription(),
                    ResultType = successResType.GetDescription()
                };

                IMessageActivity reply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResult);
                reply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard)
                    }
                };

                await context.PostAsync(reply);
            }
            else
            {
                var errorResType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = "The list may be too short, try again with more numbers.",
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