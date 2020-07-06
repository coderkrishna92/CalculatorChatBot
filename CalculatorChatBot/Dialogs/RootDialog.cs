// <copyright file="RootDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using CalculatorChatBot.BotHelpers;
    using CalculatorChatBot.Dialogs.Arithmetic;
    using CalculatorChatBot.Dialogs.Geometry;
    using CalculatorChatBot.Dialogs.Statistics;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Scorables;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Connector.Teams.Models;

    /// <summary>
    /// This is the class for the root dialog from which the execution would branch.
    /// </summary>
    [Serializable]
    public class RootDialog : DispatchDialog
    {
        /// <summary>
        /// A default help method that exists as part of practice.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <returns>A unit of execution.</returns>
        [RegexPattern(DialogMatches.HelpDialogMatch)]
        [ScorableGroup(1)]
        public static async Task GetHelp(IDialogContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            await context.PostAsync(MessageHelpers.CreateHelpMessage(string.Empty)).ConfigureAwait(false);
            context.Done<object>(null);
        }

        /// <summary>
        /// A default method that will be sent out as part of the I don't know.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <returns>A unit of execution.</returns>
        [MethodBind]
        [ScorableGroup(2)]
        public static async Task Default(IDialogContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            await context.PostAsync("I'm sorry, but I didn't understand.").ConfigureAwait(false);
            context.Done<object>(null);
        }

        /// <summary>
        /// This method would always ensure that the RootDialog would be popped off
        /// the DialogStack whenever the bot is shutting down.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="result">The awaitable object.</param>
        /// <returns>A unit of execution.</returns>
        public static async Task EndDialog(IDialogContext context, IAwaitable<object> result)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            await context.PostAsync("What else do you want to do?").ConfigureAwait(false);
            context.Done<object>(null);
        }

        /// <summary>
        /// The method that will run the hello dialog.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.HelloDialogMatch)]
        [RegexPattern(DialogMatches.HiDialogMatch)]
        [ScorableGroup(1)]
        public static void RunHelloDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var helloResult = activity as Activity;
            context.Call(new HelloDialog(helloResult), EndDialog);
        }

        /// <summary>
        /// A method that would run the greeting dialog.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <returns>A unit of execution.</returns>
        [RegexPattern(DialogMatches.GreetEveryoneDialogMatch)]
        [ScorableGroup(1)]
        public static async Task RunGreetDialog(IDialogContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var channelData = context.Activity.GetChannelData<TeamsChannelData>();
            if (channelData.Team != null)
            {
                context.Call(new GreetDialog(), EndDialog);
            }
            else
            {
                await context.PostAsync("I'm sorry, you can only do this from within a Team.").ConfigureAwait(false);
                context.Done<object>(null);
            }
        }

        /// <summary>
        /// Method that would run the add dialog.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.AddDialogMatch)]
        [RegexPattern(DialogMatches.AdditionDialogMatch)]
        [RegexPattern(DialogMatches.SumDialogMatch)]
        [ScorableGroup(1)]
        public static void RunAddDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var result = activity as Activity;
            context.Call(new AddDialog(result), EndDialog);
        }

        /// <summary>
        /// The method that runs the subtract dialog.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.SubtractDialogMatch)]
        [RegexPattern(DialogMatches.SubtractionDialogMatch)]
        [RegexPattern(DialogMatches.DifferenceDialogMatch)]
        [ScorableGroup(1)]
        public static void RunSubtractDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var result = activity as Activity;
            context.Call(new SubtractDialog(result), EndDialog);
        }

        /// <summary>
        /// The method that will run the product dialog.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.ProductDialogMatch)]
        [RegexPattern(DialogMatches.MultiplicationDialogMatch)]
        [RegexPattern(DialogMatches.MultiplyDialogMatch)]
        [ScorableGroup(1)]
        public static void RunMultiplyDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var multiResult = activity as Activity;
            context.Call(new MultiplyDialog(multiResult), EndDialog);
        }

        /// <summary>
        /// Method that will calculate the division.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.DivideDialogMatch)]
        [RegexPattern(DialogMatches.DivisionDialogMatch)]
        [RegexPattern(DialogMatches.QuotientDialogMatch)]
        [ScorableGroup(1)]
        public static void RunDivideDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var divideResult = activity as Activity;
            context.Call(new DivideDialog(divideResult), EndDialog);
        }

        /// <summary>
        /// Method that fires off the logic to calculator the remainder among a list of numbers.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.RemainderDialogMatch)]
        [RegexPattern(DialogMatches.ModuloDialogMatch)]
        [RegexPattern(DialogMatches.ModulusDialogMatch)]
        [ScorableGroup(1)]
        public static void RunModuloDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var modResult = activity as Activity;
            context.Call(new ModuloDialog(modResult), EndDialog);
        }

        /// <summary>
        /// This function calls the dialog to calculate the mean/average.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.AverageDialogMatch)]
        [RegexPattern(DialogMatches.MeanDialogMatch)]
        [ScorableGroup(1)]
        public static void RunAverageDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var averageActivity = activity as Activity;
            context.Call(new AverageDialog(averageActivity), EndDialog);
        }

        /// <summary>
        /// This will call the dialog to calculate the median.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.MedianDialogMatch1)]
        [ScorableGroup(1)]
        public static void RunMedianDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var medianActivity = activity as Activity;
            context.Call(new MedianDialog(medianActivity), EndDialog);
        }

        /// <summary>
        /// This function will call the dialog to calculate the mode.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.ModeDialogMatch1)]
        [ScorableGroup(1)]
        public static void RunModeDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var modeActivity = activity as Activity;
            context.Call(new ModeDialog(modeActivity), EndDialog);
        }

        /// <summary>
        /// The method that will fire off the range calculation.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.RangeDialogMatch1)]
        [ScorableGroup(1)]
        public static void RunRangeDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var rangeActivity = activity as Activity;
            context.Call(new RangeDialog(rangeActivity), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the dialog to calculate the variance among a list of numbers.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.VarianceDialogMatch)]
        [ScorableGroup(1)]
        public static void RunVarianceDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var varianceActivity = activity as Activity;
            context.Call(new VarianceDialog(varianceActivity), EndDialog);
        }

        /// <summary>
        /// Method that would fire off the Standard Deviation calculation.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.StandardDeviationDialogMatch1)]
        [ScorableGroup(1)]
        public static void RunStandardDeviationDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var standardDevActivity = activity as Activity;
            context.Call(new StandardDeviationDialog(standardDevActivity), EndDialog);
        }

        /// <summary>
        /// Method that would fire off the calculation of the geometric mean.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.GeometricMeanDialogMatch)]
        [ScorableGroup(1)]
        public static void RunGeometricMeanDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var geometricMeanActivity = activity as Activity;
            context.Call(new GeometricMeanDialog(geometricMeanActivity), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the Root Mean Square calculations.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.RmsDialogMatch)]
        [ScorableGroup(1)]
        public static void RunRmsDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var rmsActivity = activity as Activity;
            context.Call(new RmsDialog(rmsActivity), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the hypotenuse given 2 legs.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.PythagorasDialogMatch)]
        [RegexPattern(DialogMatches.PythagoreanDialogMatch)]
        [ScorableGroup(1)]
        public static void RunPythagoreanDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var pythagResult = activity as Activity;
            context.Call(new PythagoreanDialog(pythagResult), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the discriminant.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.NumberOfRootsDialogMatch)]
        [RegexPattern(DialogMatches.DiscriminantDialogMatch)]
        [ScorableGroup(1)]
        public static void RunDiscriminantDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var discrimResult = activity as Activity;
            context.Call(new DiscriminantDialog(discrimResult), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the roots of a quadratic equation.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.EquationRootsDialogMatch)]
        [RegexPattern(DialogMatches.QuadraticSolverDialogMatch)]
        [ScorableGroup(1)]
        public static void RunQuadraticSolverDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var quadSolverResult = activity as Activity;
            context.Call(new QuadraticSolverDialog(quadSolverResult), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the midpoint.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.MidPointDialogMatch)]
        [ScorableGroup(1)]
        public static void RunMidpointDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var midPointResult = activity as Activity;
            context.Call(new MidpointDialog(midPointResult), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the distance between 2 points.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.DistanceDialogMatch)]
        [ScorableGroup(1)]
        public static void RunDistanceDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var distanceResult = activity as Activity;
            context.Call(new DistanceDialog(distanceResult), EndDialog);
        }

        /// <summary>
        /// Method that will calculate the area of a triangle.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.TriangleAreaDialogMatch)]
        [ScorableGroup(1)]
        public static void RunTriangleAreaDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var triangleAreaResult = activity as Activity;
            context.Call(new TriangleAreaDialog(triangleAreaResult), EndDialog);
        }

        /// <summary>
        /// Method that fires off the calculation of the perimeter of a triangle.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.TrianglePerimDialogMatch)]
        [ScorableGroup(1)]
        public static void RunTrianglePerimDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var trianglePerimResult = activity as Activity;
            context.Call(new TrianglePerimDialog(trianglePerimResult), EndDialog);
        }

        /// <summary>
        /// Method that fires off the calculation of the perimeter of a quadrilateral.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.QuadrilateralPerimDialogMatch)]
        [ScorableGroup(1)]
        public static void RunQuadPerimDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var quadPerimResult = activity as Activity;
            context.Call(new QuadrilateralPerimDialog(quadPerimResult), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the area of a rectangle.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.RectangleAreaDialogMatch)]
        [ScorableGroup(1)]
        public static void RunRectangleAreaDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var rectangleAreaResult = activity as Activity;
            context.Call(new RectangleAreaDialog(rectangleAreaResult), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the area of the circle.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.CircleAreaDialogMatch)]
        [ScorableGroup(1)]
        public static void RunCircleAreaDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var circleAreaResult = activity as Activity;
            context.Call(new CircleAreaDialog(circleAreaResult), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the circumference of a circle.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.CicleCircumferenceDialogMatch)]
        [ScorableGroup(1)]
        public static void RunCircumferenceDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var circumferenceResult = activity as Activity;
            context.Call(new CircumferenceDialog(circumferenceResult), EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the area of a trapezoid.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="activity">The current activity.</param>
        [RegexPattern(DialogMatches.TrapezoidAreaDialogMatch)]
        [ScorableGroup(1)]
        public static void RunTrapezoidAreaDialog(IDialogContext context, IActivity activity)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var trapezoidAreaResult = activity as Activity;
            context.Call(new TrapezoidAreaDialog(trapezoidAreaResult), EndDialog);
        }
    }
}