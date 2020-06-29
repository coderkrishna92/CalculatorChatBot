// <copyright file="SubtractDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Arithmetic
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
    /// This class will produce the overall difference of a list of numbers.
    /// </summary>
    [Serializable]
    public class SubtractDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public SubtractDialog(Activity incomingActivity)
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
        /// This method will execute whenever this dialog executes at runtime.
        /// </summary>
        /// <param name="context">The current dialog context.</param>
        /// <returns>A unit of execution.</returns>
        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var operationType = CalculationTypes.Arithmetic;
            if (this.InputInts.Length > 1)
            {
                int diff = this.InputInts[0];
                for (int i = 1; i < this.InputInts.Length; i++)
                {
                    diff -= this.InputInts[i];
                }

                var resultType = ResultTypes.Difference;
                var results = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = diff.ToString(CultureInfo.InvariantCulture),
                    OutputMsg = $"Given the list of {this.InputString}; the difference = {diff}",
                    OperationType = operationType.GetDescription(),
                    ResultType = resultType.GetDescription(),
                };

                IMessageActivity reply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(results);
                reply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard),
                    },
                };

                await context.PostAsync(reply).ConfigureAwait(false);
            }
            else
            {
                var resultType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"The input list: {this.InputString} is too short - please provide more numbers",
                    OperationType = operationType.GetDescription(),
                    ResultType = resultType.GetDescription(),
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorAdaptiveCard),
                    },
                };

                await context.PostAsync(errorReply).ConfigureAwait(false);
            }

            // Return back to the RootDialog - popping this child dialog off the stack
            context.Done<object>(null);
        }
    }
}