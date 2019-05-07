﻿namespace CalculatorChatBot.Dialogs.Arithmetic
{
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System.Threading.Tasks;
    using System;
    using CalculatorChatBot.Models;
    using System.Collections.Generic;
    using CalculatorChatBot.Cards;

    [Serializable]
    public class DivideDialog : IDialog<object>
    {
        #region Dialog properties
        public string[] InputStringArray { get; set; }

        public string InputString { get; set; }

        public int[] InputInts { get; set; }
        #endregion

        public DivideDialog(Activity incomingActivity)
        {
            // Parsing through the necessary incoming text
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            // What is the properties to be set for the necessary 
            // operation to be performed
            if (!string.IsNullOrEmpty(incomingInfo[1]))
            {
                InputString = incomingInfo[1];

                InputStringArray = InputString.Split(',');

                InputInts = Array.ConvertAll(InputStringArray, int.Parse);
            }
        }

        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context)); 
            }

            decimal quotient = 0;
            if (InputInts.Length == 2 && InputInts[1] != 0)
            {
                quotient = Convert.ToDecimal(InputInts[0]) / InputInts[1];

                var results = new OperationResults()
                {
                    Input = InputString,
                    NumericalResult = decimal.Round(quotient, 2).ToString(),
                    OutputMsg = $"Given the list of {InputString}; the quotient = {decimal.Round(quotient, 2)}",
                    OperationType = CalculationTypes.Arithmetic.ToString(),
                    ResultType = ResultTypes.Quotient.ToString()
                };

                #region Creating the adaptive card
                IMessageActivity reply = context.MakeMessage();
                reply.Attachments = new List<Attachment>();

                var operationResultsCard = new OperationResultsCard(results);
                reply.Attachments.Add(operationResultsCard.ToAttachment());
                #endregion

                await context.PostAsync(reply);
            }
            else
            {
                var errorResults = new OperationResults()
                {
                    Input = InputString,
                    NumericalResult = "0",
                    OutputMsg = "The list may be too long, or one of the elements could be 0 - please try again later.",
                    OperationType = CalculationTypes.Arithmetic.ToString(),
                    ResultType = ResultTypes.Error.ToString()
                };

                #region Creating the adaptive card
                IMessageActivity errorReply = context.MakeMessage();
                errorReply.Attachments = new List<Attachment>();

                var errorCard = new OperationErrorCard(errorResults);
                errorReply.Attachments.Add(errorCard.ToAttachment());
                #endregion

                // Send the message that you need more elements to calculate the sum
                await context.PostAsync(errorReply);
            }

            // Return back to the root dialog - popping this child dialog from the dialog stack
            context.Done<object>(null);
        }
    }
}