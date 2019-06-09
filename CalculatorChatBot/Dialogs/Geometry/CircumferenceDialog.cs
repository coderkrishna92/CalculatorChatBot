// <copyright file="CircumferenceDialog.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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

    public class CircumferenceDialog : IDialog<object>
    {
        public CircumferenceDialog(Activity incomingActivity)
        {
            // Parsing through the incoming message text
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            // Which properties are being set for the operations to be performed
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
            if (this.InputInts.Length != 1)
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
            else if (this.InputInts.Length == 1 && this.InputInts[0] == 0)
            {
                var noCircumError = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"The input list: {this.InputString} may not be valid. Please try again",
                    OperationType = operationType.GetDescription(),
                    ResultType = noCircumError.GetDescription()
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
                var circumferenceResult = Convert.ToDecimal(2 * Math.PI * this.InputInts[0]);
                var successResultType = ResultTypes.Circumference;
                var successResult = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(circumferenceResult, 2).ToString(), 
                    OutputMsg = $"Given the input: {this.InputString}, the circumference = {decimal.Round(circumferenceResult, 2).ToString()}", 
                    OperationType = operationType.GetDescription(),
                    ResultType = successResultType.GetDescription()
                };

                IMessageActivity successReply = context.MakeMessage();
                var successAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResult);
                successReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(successAdaptiveCard)
                    }
                };

                await context.PostAsync(successReply);
            }
        }
    }
}