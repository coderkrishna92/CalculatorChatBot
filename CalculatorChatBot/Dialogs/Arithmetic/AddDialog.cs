// <copyright file="AddDialog.cs" company="XYZ Software LLC">
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

    /// <summary>
    /// This class will produce the overall sum of a list of numbers. If the list is too short, the
    /// bot will reply with an appropriate message.
    /// </summary>
    [Serializable]
    public class AddDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public AddDialog(Activity incomingActivity)
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
            var calculationType = CalculationTypes.Arithmetic;

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (this.InputInts.Length > 1)
            {
                int sum = this.InputInts[0];
                for (int i = 1; i < this.InputInts.Length; i++)
                {
                    sum += this.InputInts[i];
                }

                var resType = ResultTypes.Sum;
                var results = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = sum.ToString(),
                    OutputMsg = $"Given the list of {this.InputString}; the sum = {sum}",
                    OperationType = calculationType.GetDescription(),
                    ResultType = resType.GetDescription()
                };

                IMessageActivity reply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(results);
                reply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard)
                    }
                };
                await context.PostAsync(reply);
            }
            else
            {
                var errorResType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"The input list: {this.InputString} is too short - please provide more numbers",
                    OperationType = calculationType.GetDescription(),
                    ResultType = errorResType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorAdaptiveCard)
                    }
                };

                await context.PostAsync(errorReply);
            }

            context.Done<object>(null);
        }
    }
}