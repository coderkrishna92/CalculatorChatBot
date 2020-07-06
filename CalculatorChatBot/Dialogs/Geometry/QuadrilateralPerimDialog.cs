// <copyright file="QuadrilateralPerimDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Geometry
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;

    /// <summary>
    /// Given a list of 4 integers, this dialog will calculate the perimeter of a quadrilateral.
    /// </summary>
    [Serializable]
    public class QuadrilateralPerimDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuadrilateralPerimDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public QuadrilateralPerimDialog(Activity incomingActivity)
        {
            if (incomingActivity is null)
            {
                throw new ArgumentNullException(nameof(incomingActivity));
            }

            // Parsing through the incoming message text
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
        /// This method executes whenever this dialog is running at runtime.
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
            if (this.InputInts.Length != 4)
            {
                var errorResultType = ResultTypes.Error;
                var errorResult = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"Your input: {this.InputString} is not valid. Please try again later!",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResultType.GetDescription(),
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResult);
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
            else
            {
                // Looking to see if all the values are the same
                var isSquare = this.InputInts[0] == this.InputInts[1] && this.InputInts[1] == this.InputInts[2] && this.InputInts[2] == this.InputInts[3];

                if (!isSquare)
                {
                    var totalPerimeter = this.InputInts.Sum();
                    var totalPerimResultType = ResultTypes.QuadPerimeter;

                    var totalPerimResult = new OperationResults()
                    {
                        Input = this.InputString,
                        NumericalResult = totalPerimeter.ToString(CultureInfo.InvariantCulture),
                        OperationType = operationType.GetDescription(),
                        OutputMsg = $"Given the input list: {this.InputString}, the perimeter = {totalPerimeter}",
                        ResultType = totalPerimResultType.GetDescription(),
                    };

                    IMessageActivity perimeterReply = context.MakeMessage();
                    var totalPerimAdaptiveCard = OperationResultsAdaptiveCard.GetCard(totalPerimResult);
                    perimeterReply.Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = "application/vnd.microsoft.card.adaptive",
                            Content = JsonConvert.DeserializeObject(totalPerimAdaptiveCard),
                        },
                    };

                    await context.PostAsync(perimeterReply).ConfigureAwait(false);
                }
                else
                {
                    var squarePerimeter = 4 * this.InputInts[0];
                    var totalPerimResultType = ResultTypes.QuadPerimeter;

                    var squarePerimResult = new OperationResults()
                    {
                        Input = this.InputString,
                        NumericalResult = squarePerimeter.ToString(CultureInfo.InvariantCulture),
                        OperationType = operationType.GetDescription(),
                        OutputMsg = $"Given the input list: {this.InputString}, the perimeter = {squarePerimeter}",
                        ResultType = totalPerimResultType.GetDescription(),
                    };

                    IMessageActivity squarePerimReply = context.MakeMessage();
                    var squarePerimAdaptiveCard = OperationResultsAdaptiveCard.GetCard(squarePerimResult);
                    squarePerimReply.Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = "application/vnd.microsoft.card.adaptive",
                            Content = JsonConvert.DeserializeObject(squarePerimAdaptiveCard),
                        },
                    };

                    await context.PostAsync(squarePerimReply).ConfigureAwait(false);
                }
            }
        }
    }
}