// <copyright file="PythagoreanDialog.cs" company="XYZ Software LLC">
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
    /// Given a list of 2 integers that represents the legs of a right triangle, the hypotenuse
    /// will be returned.
    /// </summary>
    [Serializable]
    public class PythagoreanDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PythagoreanDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public PythagoreanDialog(Activity incomingActivity)
        {
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            if (!string.IsNullOrEmpty(incomingInfo[1]))
            {
                this.InputString = incomingInfo[1];
                this.InputStringArray = this.InputString.Split(',');
                this.InputInts = Array.ConvertAll(this.InputStringArray, int.Parse);
            }
        }

        public string[] InputStringArray { get; set; }

        public string InputString { get; set; }

        public int[] InputInts { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var operationType = CalculationTypes.Geometric;
            if (this.InputInts.Length > 2)
            {
                var errorResultType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"The input list: {this.InputString} is too long. I need only 2 numbers to find the length of the hypotenuse",
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
            else
            {
                // Having the necessary calculations done
                double a = Convert.ToDouble(this.InputInts[0]);
                double b = Convert.ToDouble(this.InputInts[1]);

                var hypotenuseSqr = Math.Pow(a, 2) + Math.Pow(b, 2);

                double c = Math.Sqrt(hypotenuseSqr);

                var output = $"Given the legs of ${this.InputInts[0]} and ${this.InputInts[1]}, the hypotenuse of the right triangle is ${decimal.Round(decimal.Parse(c.ToString()), 2)}";

                var resultType = ResultTypes.Hypotenuse;
                var successResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(decimal.Parse(c.ToString()), 2).ToString(),
                    OutputMsg = output,
                    OperationType = operationType.GetDescription(),
                    ResultType = resultType.GetDescription()
                };

                IMessageActivity successReply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResults);
                successReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard)
                    }
                };
                await context.PostAsync(successReply);
            }

            context.Done<object>(null);
        }
    }
}