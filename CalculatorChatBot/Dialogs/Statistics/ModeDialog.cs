// <copyright file="ModeDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Statistics
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
    /// Given a list of integers, this dialog will calculate the mode of the list. Mode = most often.
    /// </summary>
    [Serializable]
    public class ModeDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModeDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public ModeDialog(Activity incomingActivity)
        {
            if (incomingActivity is null)
            {
                throw new ArgumentNullException(nameof(incomingActivity));
            }

            // Extract the incoming text/message
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            if (!string.IsNullOrEmpty(incomingInfo[1]))
            {
                this.InputString = incomingInfo[1];
                this.InputStringArray = this.InputString.Split(',');
                this.InputInts = Array.ConvertAll(this.InputStringArray, int.Parse);
            }
        }

        /// <summary>
        /// Gets or sets the input string.
        /// </summary>
        public string InputString { get; set; }

        /// <summary>
        /// Gets or sets the input string array.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public string[] InputStringArray { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// Gets or sets the input integers.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public int[] InputInts { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// This method will execute when this dialog is running.
        /// </summary>
        /// <param name="context">The current dialog context.</param>
        /// <returns>A unit of execution.</returns>
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
                                Count = groupedNumbers.Count(),
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
                    NumericalResult = outputArray.Length > 1 ? string.Join(",", outputArray) : outputArray[0].ToString(CultureInfo.InvariantCulture),
                    OutputMsg = $"Given the list: {this.InputString}; the mode = {(outputArray.Length > 1 ? string.Join(",", outputArray) : outputArray[0].ToString(CultureInfo.InvariantCulture))}",
                    OperationType = operationType.GetDescription(),
                    ResultType = successResType.GetDescription(),
                };

                IMessageActivity successReply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResult);
                successReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard),
                    },
                };

                await context.PostAsync(successReply).ConfigureAwait(false);
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
                    ResultType = errorResType.GetDescription(),
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorReplyAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResult);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorReplyAdaptiveCard),
                    },
                };

                await context.PostAsync(errorReply).ConfigureAwait(false);
            }

            context.Done<object>(null);
        }
    }
}