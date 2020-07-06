// <copyright file="TriangleAreaDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Geometry
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
    /// Given a list of 2 integers representing the base and height, this dialog calculates the
    /// area of a triangle.
    /// </summary>
    [Serializable]
    public class TriangleAreaDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriangleAreaDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public TriangleAreaDialog(Activity incomingActivity)
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
        /// This method executes whenever this dialog is being executed.
        /// </summary>
        /// <param name="context">The current dialog context.</param>
        /// <returns>A unit of execution.</returns>
        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var operationType = CalculationTypes.Geometric;
            if (this.InputInts.Length == 2 && (this.InputInts[0] == 0 || this.InputInts[1] == 0))
            {
                var errorResultType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"The input list: {this.InputString} may not be valid. Please try again",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResultType.GetDescription(),
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorResultsAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorResultsAdaptiveCard),
                    },
                };

                await context.PostAsync(errorReply).ConfigureAwait(false);
            }
            else
            {
                var triangleAreaResult = Convert.ToDecimal(0.5 * this.InputInts[0] * this.InputInts[1]);

                var successResultType = ResultTypes.TriangleArea;
                var successResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(triangleAreaResult, 2).ToString(CultureInfo.InvariantCulture),
                    OutputMsg = $"Given the inputs: {this.InputString}, the output = {decimal.Round(triangleAreaResult, 2)}",
                    OperationType = operationType.GetDescription(),
                    ResultType = successResultType.GetDescription(),
                };

                IMessageActivity successReply = context.MakeMessage();
                var successResultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResults);
                successReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(successResultsAdaptiveCard),
                    },
                };

                await context.PostAsync(successReply).ConfigureAwait(false);
            }

            context.Done<object>(null);
        }
    }
}