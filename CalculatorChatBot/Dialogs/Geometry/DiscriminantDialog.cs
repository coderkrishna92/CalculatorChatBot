// <copyright file="DiscriminantDialog.cs" company="XYZ Software LLC">
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
    /// This class will calculate the discriminant value of a quadratic equation - give the values of
    /// A, B, and C.
    /// </summary>
    [Serializable]
    public class DiscriminantDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscriminantDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public DiscriminantDialog(Activity incomingActivity)
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
            if (this.InputInts.Length > 3)
            {
                var errorListTooLongResType = ResultTypes.Error;
                var errorListTooLongResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "DNE",
                    OutputMsg = $"The input list: {this.InputString} could be too long - there needs to be 3 numbers exactly",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorListTooLongResType.GetDescription()
                };

                IMessageActivity errorListTooLongReply = context.MakeMessage();
                var errorListTooLongAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorListTooLongResults);
                errorListTooLongReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorListTooLongAdaptiveCard)
                    }
                };
                await context.PostAsync(errorListTooLongReply);
            }
            else if (this.InputInts.Length < 3)
            {
                var errorListTooShortResType = ResultTypes.Error;
                var errorListTooShortResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "DNE",
                    OutputMsg = $"The input list: {this.InputString} could be too short - there needs to be 3 numbers exactly",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorListTooShortResType.GetDescription()
                };

                IMessageActivity errorListTooShortReply = context.MakeMessage();
                var errorListTooShortAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorListTooShortResults);
                errorListTooShortReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorListTooShortAdaptiveCard)
                    }
                };
                await context.PostAsync(errorListTooShortReply);
            }
            else
            {
                int a = this.InputInts[0];
                int b = this.InputInts[1];
                int c = this.InputInts[2];

                int discriminantValue = FindDiscriminant(a, b, c);
                var resultMsg = string.Empty;
                var resultsType = ResultTypes.Discriminant;

                if (discriminantValue > 0)
                {
                    resultMsg = $"Given your values: a = {a}, b = {b}, c = {c} - the discriminant = {discriminantValue} which means there are 2 roots";
                }
                else if (discriminantValue == 0)
                {
                    resultMsg = $"Given your values: a = {a}, b = {b}, c = {c} - the discriminant = {discriminantValue} which means there is 1 root";
                }
                else
                {
                    resultMsg = $"Given your values: a = {a}, b = {b}, c = {c} - the discriminant = {discriminantValue} which means there are no real roots";
                }

                var discrimResults = new OperationResults()
                {
                    Input = this.InputString,
                    OperationType = operationType.GetDescription(),
                    OutputMsg = resultMsg,
                    NumericalResult = discriminantValue.ToString(),
                    ResultType = resultsType.GetDescription()
                };

                IMessageActivity discrimReply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(discrimResults);
                discrimReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard)
                    }
                };

                await context.PostAsync(discrimReply);
            }

            context.Done<object>(null);
        }

        private static int FindDiscriminant(int a, int b, int c)
        {
            int disc = (int)Math.Pow(b, 2) - (4 * a * c);
            return disc;
        }
    }
}