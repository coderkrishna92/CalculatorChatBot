// <copyright file="QuadraticSolverDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Geometry
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;

    /// <summary>
    /// Given a list of 3 integers which represent A, B, and C; this dialog returns the roots.
    /// </summary>
    [Serializable]
    public class QuadraticSolverDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuadraticSolverDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public QuadraticSolverDialog(Activity incomingActivity)
        {
            if (incomingActivity is null)
            {
                throw new ArgumentNullException(nameof(incomingActivity));
            }

            string[] incomingInfo = incomingActivity.Text.Split(' ');

            if (!string.IsNullOrEmpty(incomingInfo[1]))
            {
                this.InputString = incomingInfo[1];
                this.InputStringArray = this.InputString.Split(',');
                this.InputInts = Array.ConvertAll(this.InputStringArray, int.Parse);
            }
        }

        /// <summary>
        /// Gets or sets the input string array.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public string[] InputStringArray { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// Gets or sets the input string.
        /// </summary>
        public string InputString { get; set; }

        /// <summary>
        /// Gets or sets the input integers.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public int[] InputInts { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// This method will run whenever this dialog is executing.
        /// </summary>
        /// <param name="context">The current dialog context.</param>
        /// <returns>A unit of execution.</returns>
        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var operationType = CalculationTypes.Geometric;
            if (this.InputInts.Length > 3)
            {
                var errorResType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = "Your list may be too large to calculate the roots. Please try again later!",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResType.GetDescription(),
                };

                IMessageActivity errorListReply = context.MakeMessage();
                var errorListAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorListReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorListAdaptiveCard),
                    },
                };
                await context.PostAsync(errorListReply).ConfigureAwait(false);
            }
            else
            {
                double a = Convert.ToDouble(this.InputInts[0]);
                double b = Convert.ToDouble(this.InputInts[1]);
                double c = Convert.ToDouble(this.InputInts[2]);

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
                        var opsErrorResultType = ResultTypes.Error;
                        var opsError = new OperationResults()
                        {
                            Input = this.InputString,
                            NumericalResult = "0",
                            OutputMsg = "The information provided may lead to a linear equation!",
                            OperationType = CalculationTypes.Geometric.ToString(),
                            ResultType = opsErrorResultType.GetDescription(),
                        };

                        IMessageActivity opsErrorReply = context.MakeMessage();
                        var opsErrorAdaptiveCard = OperationErrorAdaptiveCard.GetCard(opsError);
                        opsErrorReply.Attachments = new List<Attachment>()
                        {
                            new Attachment()
                            {
                                ContentType = "application/vnd.microsoft.card.adaptive",
                                Content = JsonConvert.DeserializeObject(opsErrorAdaptiveCard),
                            },
                        };
                        await context.PostAsync(opsErrorReply).ConfigureAwait(false);
                        break;
                    case 2:
                        r1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                        r2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

                        var successResultType = ResultTypes.EquationRoots;
                        var successResults = new OperationResults()
                        {
                            Input = this.InputString,
                            NumericalResult = $"{r1}, {r2}",
                            OutputMsg = $"The roots are Real and Distinct - Given the list of: {this.InputString}, the roots are [{r1}, {r2}]",
                            OperationType = operationType.GetDescription(),
                            ResultType = successResultType.GetDescription(),
                        };

                        IMessageActivity opsSuccessReply = context.MakeMessage();
                        var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResults);
                        opsSuccessReply.Attachments = new List<Attachment>()
                        {
                            new Attachment()
                            {
                                ContentType = "application/vnd.microsoft.card.adaptive",
                                Content = JsonConvert.DeserializeObject(resultsAdaptiveCard),
                            },
                        };

                        await context.PostAsync(opsSuccessReply).ConfigureAwait(false);
                        break;
                    case 3:
                        r1 = r2 = (-b) / (2 * a);

                        var successOpsOneRootResultType = ResultTypes.EquationRoots;
                        var successOpsOneRoot = new OperationResults()
                        {
                            Input = this.InputString,
                            NumericalResult = $"{r1}, {r2}",
                            OutputMsg = $"The roots are Real and Distinct - Given the list of: {this.InputString}, the roots are [{r1}, {r2}]",
                            OperationType = operationType.GetDescription(),
                            ResultType = successOpsOneRootResultType.GetDescription(),
                        };

                        IMessageActivity opsSuccessOneRootReply = context.MakeMessage();
                        var successOpsOneRootAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successOpsOneRoot);
                        opsSuccessOneRootReply.Attachments = new List<Attachment>()
                        {
                            new Attachment()
                            {
                                ContentType = "application/vnd.microsoft.card.adaptive",
                                Content = JsonConvert.DeserializeObject(successOpsOneRootAdaptiveCard),
                            },
                        };

                        await context.PostAsync(opsSuccessOneRootReply).ConfigureAwait(false);
                        break;
                    case 4:
                        var rootsDesc = "Roots are imaginary";
                        r1 = (-b) / (2 * a);
                        r2 = Math.Sqrt(-discriminant) / (2 * a);

                        var root1Str = string.Format(CultureInfo.InvariantCulture, "First root is {0:#.##} + {1:#.##}i", r1, r2);
                        var root2Str = string.Format(CultureInfo.InvariantCulture, "Second root is {0:#.##} - {1:#.##}i", r1, r2);

                        var root1 = string.Format(CultureInfo.InvariantCulture, "{0:#.##} + {1:#.##}i", r1, r2);
                        var root2 = string.Format(CultureInfo.InvariantCulture, "{0:#.##} - {1:#.##}i", r1, r2);

                        var imaginaryRootsResult = ResultTypes.EquationRoots;
                        var opsSuccessImaginRootsResults = new OperationResults()
                        {
                            Input = this.InputString,
                            NumericalResult = $"{root1}, {root2}",
                            OutputMsg = rootsDesc + " " + root1Str + " " + root2Str,
                            OperationType = operationType.GetDescription(),
                            ResultType = imaginaryRootsResult.GetDescription(),
                        };

                        IMessageActivity opsSuccessImagReply = context.MakeMessage();
                        var opsSuccessImaginRootCard = OperationResultsAdaptiveCard.GetCard(opsSuccessImaginRootsResults);
                        opsSuccessImagReply.Attachments = new List<Attachment>()
                        {
                            new Attachment()
                            {
                                ContentType = "application/vnd.microsoft.card.adaptive",
                                Content = JsonConvert.DeserializeObject(opsSuccessImaginRootCard),
                            },
                        };

                        await context.PostAsync(opsSuccessImagReply).ConfigureAwait(false);
                        break;
                    default:
                        await context.PostAsync("Sorry I'm not sure what is going on here").ConfigureAwait(false);
                        break;
                }
            }

            context.Done<object>(null);
        }
    }
}