// <copyright file="ModeDialog.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;

    [Serializable]
    public class ModeDialog : IDialog<object>
    {
        public ModeDialog(Activity incomingActivity)
        {
            // Extract the incoming text/message
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

            var operationType = CalculationTypes.Statistical;

            if (this.InputInts.Length > 2)
            {
                List<int> originalList = new List<int>(this.InputInts);
                List<int> modesList = new List<int>();

                // Using LINQ in the lines below
                var query = from numbers in originalList
                            group numbers by numbers
                                into groupedNumbers
                            select new
                            {
                                Number = groupedNumbers.Key,
                                Count = groupedNumbers.Count()
                            };

                int max = query.Max(g => g.Count);

                if (max == 1)
                {
                    int mode = 0;
                    modesList.Add(mode);
                }
                else
                {
                    modesList = query.Where(x => x.Count == max).Select(x => x.Number).ToList();
                }

                var outputArray = modesList.ToArray();
                var successResType = ResultTypes.Mode;

                var successResult = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = outputArray.Length > 1 ? string.Join(",", outputArray) : outputArray[0].ToString(),
                    OutputMsg = $"Given the list: {this.InputString}; the mode = {(outputArray.Length > 1 ? string.Join(",", outputArray) : outputArray[0].ToString())}",
                    OperationType = operationType.GetDescription(),
                    ResultType = successResType.GetDescription()
                };

                IMessageActivity successReply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResult);
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
            else
            {
                var errorResType = ResultTypes.Error;
                var errorResult = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"Please check your input list: {this.InputString} and try again later",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorReplyAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResult);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorReplyAdaptiveCard)
                    }
                };
                await context.PostAsync(errorReply);
            }

            context.Done<object>(null);
        }
    }
}