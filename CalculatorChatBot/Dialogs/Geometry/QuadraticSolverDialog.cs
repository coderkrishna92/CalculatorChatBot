﻿namespace CalculatorChatBot.Dialogs.Geometry
{
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class QuadraticSolverDialog : IDialog<object>
    {
        #region Dialog properties
        public string[] InputStringArray { get; set; }

        public string InputString { get; set; }

        public int[] InputInts { get; set; }
        #endregion

        public QuadraticSolverDialog(Activity incomingActivity)
        {
            string[] incomingInfo = incomingActivity.Text.Split(' ');

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

            if (InputInts.Length > 3)
            {
                var errorResults = new OperationResults()
                {
                    Input = InputString, 
                    Output = "0",
                    OutputMsg = "Your list may be too large to calculate the roots. Please try again later!",
                    OperationType = CalculationTypes.Geometric.ToString(), 
                    ResultType = ResultTypes.Error.ToString()
                };

                IMessageActivity errorListReply = context.MakeMessage();
                errorListReply.Attachments = new List<Attachment>();

                var errorListCard = new OperationErrorCard(errorResults);
                errorListReply.Attachments.Add(errorListCard.ToAttachment());

                await context.PostAsync(errorListReply);
            }
            else
            {
                double a = Convert.ToDouble(InputInts[0]);
                double b = Convert.ToDouble(InputInts[1]);
                double c = Convert.ToDouble(InputInts[2]);

                var opsSuccess, opsError, opsSuccessCard, opsErrorCard, opsSuccessReply, opsErrorReply;

                // The two roots of the quadratic equation
                double r1, r2;

                var discriminant = Math.Pow(b, 2) - (4 * a * c);

                int m;

                if (a == 0)
                {
                    m = 1;
                }
                else if (discriminant > 0)
                {
                    m = 2;
                }
                else if (discriminant == 0)
                {
                    m = 3;
                }
                else
                {
                    m = 4;
                }

                switch (m)
                {
                    case 1:
                        opsError = new OperationResults()
                        {
                            Input = InputString,
                            Output = "0",
                            OutputMsg = "The information provided may lead to a linear equation!",
                            OperationType = CalculationTypes.Geometric.ToString(),
                            ResultType = ResultTypes.Error.ToString()
                        };

                        opsErrorReply = context.MakeMessage();
                        opsErrorReply.Attachments = new List<Attachment>();

                        opsErrorCard = new OperationErrorCard(opsError);
                        opsErrorReply.Attachments.Add(opsErrorCard.ToAttachment());

                        await context.PostAsync(opsErrorReply);
                        break;
                    case 2:

                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        await context.PostAsync("Sorry I'm not sure what is going on here");
                        break;
                }
            }

            context.Done<object>(null);
        }
    }
}