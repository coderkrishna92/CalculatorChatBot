// <copyright file="ModuloDialog.cs" company="XYZ Software Company LLC">
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
    /// This is the dialog for conducting the modulus operation.
    /// </summary>
    [Serializable]
    public class ModuloDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuloDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public ModuloDialog(Activity incomingActivity)
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
        /// This method executes whenever this dialog is run.
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
            if (this.InputInts.Length == 2 && this.InputInts[1] != 0)
            {
                int remainder = this.InputInts[0] % this.InputInts[1];

                var results = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = remainder.ToString(CultureInfo.InvariantCulture),
                    OutputMsg = $"Given the list {this.InputString}; the remainder = {remainder}",
                    OperationType = operationType.GetDescription(),
                    ResultType = ResultTypes.Remainder.ToString(),
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
                var errorResultType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"The list: {this.InputString} may be invalid for this operation. Please double check, and try again",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResultType.GetDescription(),
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

            context.Done<object>(null);
        }
    }
}