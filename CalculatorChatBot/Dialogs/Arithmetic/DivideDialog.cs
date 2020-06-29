// <copyright file="DivideDialog.cs" company="XYZ Software Company LLC">
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
    /// This class will produce the quotient of two numbers.
    /// </summary>
    [Serializable]
    public class DivideDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivideDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public DivideDialog(Activity incomingActivity)
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
        /// Gets or sets the input string array.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public string[] InputStringArray { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// Gets or sets the input string.
        /// </summary>
        public string InputString { get; set; }

        /// <summary>
        /// Gets or sets the input integers.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public int[] InputInts { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// The method which executes when the dialog is running.
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
            decimal quotient = 0;
            if (this.InputInts.Length == 2 && this.InputInts[1] != 0)
            {
                quotient = Convert.ToDecimal(this.InputInts[0]) / this.InputInts[1];
                var resultsType = ResultTypes.Quotient;

                var results = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(quotient, 2).ToString(CultureInfo.InvariantCulture),
                    OutputMsg = $"Given the list of {this.InputString}; the quotient = {decimal.Round(quotient, 2)}",
                    OperationType = operationType.GetDescription(),
                    ResultType = resultsType.GetDescription(),
                };

                IMessageActivity reply = context.MakeMessage();
                var resultsCard = OperationResultsAdaptiveCard.GetCard(results);
                reply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsCard),
                    },
                };
                await context.PostAsync(reply).ConfigureAwait(false);
            }
            else
            {
                var errorType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = "The list may be too long, or one of the elements could be 0 - please try again later.",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorType.GetDescription(),
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorResultsCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorResultsCard),
                    },
                };

                await context.PostAsync(errorReply).ConfigureAwait(false);
            }

            context.Done<object>(null);
        }
    }
}