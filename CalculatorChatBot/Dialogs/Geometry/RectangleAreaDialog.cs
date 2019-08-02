// <copyright file="RectangleAreaDialog.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Geometry
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
    /// Given a list of 2 integers that represent the length and width, this dialog calculates the area
    /// of a rectangle or square.
    /// </summary>
    [Serializable]
    public class RectangleAreaDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleAreaDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public RectangleAreaDialog(Activity incomingActivity)
        {
            // Parsing through the incoming message text
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

            var operationType = CalculationTypes.Geometric;
            if (this.InputInts.Length != 2)
            {
                var errorResultType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"The input list: {this.InputString} may not be valid. Please try again",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResultType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorResultsAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorResultsAdaptiveCard)
                    }
                };

                await context.PostAsync(errorReply);
            }
            else if (this.InputInts[0] == this.InputInts[1])
            {
                var squareAreaResult = Convert.ToDecimal(Math.Pow(this.InputInts[0], 2));

                var successResultType = ResultTypes.SquareArea;
                var successResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(squareAreaResult, 2).ToString(),
                    OutputMsg = $"Given the inputs: {this.InputString}, the output = {decimal.Round(squareAreaResult, 2).ToString()}",
                    OperationType = operationType.GetDescription(),
                    ResultType = successResultType.GetDescription()
                };

                IMessageActivity squareSuccessReply = context.MakeMessage();
                var successResultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResults);
                squareSuccessReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(successResultsAdaptiveCard)
                    }
                };

                await context.PostAsync(squareSuccessReply);
            }
            else
            {
                var rectangleAreaResult = Convert.ToDecimal(this.InputInts[0] * this.InputInts[1]);

                var successResultType = ResultTypes.RectangleArea;
                var successResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(rectangleAreaResult, 2).ToString(),
                    OutputMsg = $"Given the inputs: {this.InputString}, the output = {decimal.Round(rectangleAreaResult, 2).ToString()}",
                    OperationType = operationType.GetDescription(),
                    ResultType = successResultType.GetDescription()
                };

                IMessageActivity successReply = context.MakeMessage();
                var successResultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResults);
                successReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(successResultsAdaptiveCard)
                    }
                };

                await context.PostAsync(successReply);
            }

            context.Done<object>(null);
        }
    }
}