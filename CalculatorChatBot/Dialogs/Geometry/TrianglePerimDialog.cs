// <copyright file="TrianglePerimDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
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

    /// <summary>
    /// Given the list of 3 integers, this dialog calculates the perimeter of a triangle.
    /// </summary>
    [Serializable]
    public class TrianglePerimDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrianglePerimDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public TrianglePerimDialog(Activity incomingActivity)
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
            if (this.InputInts.Length != 3)
            {
                var errorResultType = ResultTypes.Error;
                var errorResult = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OperationType = operationType.GetDescription(),
                    OutputMsg = $"Your input list: {this.InputString} is not valid, please check the input list and try again",
                    ResultType = errorResultType.GetDescription(),
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResult);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorAdaptiveCard),
                    },
                };

                await context.PostAsync(errorReply);
            }
            else
            {
                var isEquilateral = this.InputInts[0] == this.InputInts[1] && this.InputInts[1] == this.InputInts[2] && this.InputInts[0] == this.InputInts[2];

                // Add all sides - for the scalene and isoceles cases
                if (!isEquilateral)
                {
                    var perimeter = this.InputInts.Sum();
                    var perimResultType = ResultTypes.TrianglePerimeter;
                    var perimResults = new OperationResults()
                    {
                        Input = this.InputString,
                        NumericalResult = perimeter.ToString(),
                        OperationType = operationType.GetDescription(),
                        OutputMsg = $"Given the inputs: {this.InputString}, the perimeter = {perimeter}",
                        ResultType = perimResultType.GetDescription(),
                    };

                    IMessageActivity perimSuccessReply = context.MakeMessage();
                    var perimSuccessAdaptiveCard = OperationResultsAdaptiveCard.GetCard(perimResults);
                    perimSuccessReply.Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = "application/vnd.microsoft.card.adaptive",
                            Content = JsonConvert.DeserializeObject(perimSuccessAdaptiveCard),
                        },
                    };

                    await context.PostAsync(perimSuccessReply);
                }
                else
                {
                    var equiPerim = 3 * this.InputInts[0];
                    var perimResultType = ResultTypes.TrianglePerimeter;
                    var perimResults = new OperationResults()
                    {
                        Input = this.InputString,
                        NumericalResult = equiPerim.ToString(),
                        OperationType = operationType.GetDescription(),
                        OutputMsg = $"Given the inputs: {this.InputString}, the perimeter = {equiPerim}",
                        ResultType = perimResultType.GetDescription(),
                    };

                    IMessageActivity equiPerimSuccessReply = context.MakeMessage();
                    var perimSuccessAdaptiveCard = OperationResultsAdaptiveCard.GetCard(perimResults);
                    equiPerimSuccessReply.Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = "application/vnd.microsoft.card.adaptive",
                            Content = JsonConvert.DeserializeObject(perimSuccessAdaptiveCard),
                        },
                    };

                    await context.PostAsync(equiPerimSuccessReply);
                }

                context.Done<object>(null);
            }
        }
    }
}