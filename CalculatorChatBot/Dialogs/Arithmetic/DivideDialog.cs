// <copyright file="DivideDialog.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Arithmetic
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;

    [Serializable]
    public class DivideDialog : IDialog<object>
    {
        public DivideDialog(Activity incomingActivity)
        {
            // Parsing through the necessary incoming text
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            // What is the properties to be set for the necessary
            // operation to be performed
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

            var operationType = CalculationTypes.Arithmetic;
            decimal quotient = 0;
            if (this.InputInts.Length == 2 && this.InputInts[1] != 0)
            {
                quotient = Convert.ToDecimal(this.InputInts[0]) / this.InputInts[1];
                var resultsType = ResultTypes.Quotient;

                var results = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(quotient, 2).ToString(),
                    OutputMsg = $"Given the list of {this.InputString}; the quotient = {decimal.Round(quotient, 2)}",
                    OperationType = operationType.GetDescription(),
                    ResultType = resultsType.GetDescription()
                };

                IMessageActivity reply = context.MakeMessage();
                var resultsCard = OperationResultsAdaptiveCard.GetCard(results);
                reply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsCard)
                    }
                };
                await context.PostAsync(reply);
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
                    ResultType = errorType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorResultsCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorResultsCard)
                    }
                };

                await context.PostAsync(errorReply);
            }

            // Return back to the root dialog - popping this child dialog from the dialog stack
            context.Done<object>(null);
        }
    }
}