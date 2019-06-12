// <copyright file="QuadrilateralPerimDialog.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Geometry
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
    public class QuadrilateralPerimDialog : IDialog<object>
    {
        public QuadrilateralPerimDialog(Activity incomingActivity)
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
            if (this.InputInts.Length != 4)
            {
                var errorResultType = ResultTypes.Error;
                var errorResult = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = $"Your input: {this.InputString} is not valid. Please try again later!",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResultType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResult);
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
                        NumericalResult = totalPerimeter.ToString(),
                        OperationType = operationType.GetDescription(),
                        OutputMsg = $"Given the input list: {this.InputString}, the perimeter = {totalPerimeter}",
                        ResultType = totalPerimResultType.GetDescription()
                    };

                    IMessageActivity perimeterReply = context.MakeMessage();
                    var totalPerimAdaptiveCard = OperationResultsAdaptiveCard.GetCard(totalPerimResult);
                    perimeterReply.Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = "application/vnd.microsoft.card.adaptive",
                            Content = JsonConvert.DeserializeObject(totalPerimAdaptiveCard)
                        }
                    };

                    await context.PostAsync(perimeterReply);
                }
                else
                {
                    var squarePerimeter = 4 * this.InputInts[0];
                    var totalPerimResultType = ResultTypes.QuadPerimeter;

                    var squarePerimResult = new OperationResults()
                    {
                        Input = this.InputString,
                        NumericalResult = squarePerimeter.ToString(),
                        OperationType = operationType.GetDescription(),
                        OutputMsg = $"Given the input list: {this.InputString}, the perimeter = {squarePerimeter}",
                        ResultType = totalPerimResultType.GetDescription()
                    };

                    IMessageActivity squarePerimReply = context.MakeMessage();
                    var squarePerimAdaptiveCard = OperationResultsAdaptiveCard.GetCard(squarePerimResult);
                    squarePerimReply.Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = "application/vnd.microsoft.card.adaptive",
                            Content = JsonConvert.DeserializeObject(squarePerimAdaptiveCard)
                        }
                    };

                    await context.PostAsync(squarePerimReply);
                }
            }
        }
    }
}