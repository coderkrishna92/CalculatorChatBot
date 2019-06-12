// <copyright file="RootDialog.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
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

    [Serializable]
    public class RootDialog : DispatchDialog
    {
        /// <summary>
        /// The method that will run the hello dialog
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.HelloDialogMatch)]
        [RegexPattern(DialogMatches.HiDialogMatch)]
        [ScorableGroup(1)]
        public void RunHelloDialog(IDialogContext context, IActivity activity)
        {
            var helloResult = activity as Activity;
            context.Call(new HelloDialog(helloResult), this.EndDialog);
        }

        /// <summary>
        /// A method that would run the greeting dialog
        /// </summary>
        /// <param name="context">The current context</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.GreetEveryoneDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunGreetDialog(IDialogContext context)
        {
            var channelData = context.Activity.GetChannelData<TeamsChannelData>();
            if (channelData.Team != null)
            {
                context.Call(new GreetDialog(), this.EndDialog);
            }
            else
            {
                await context.PostAsync("I'm sorry, you can only do this from within a Team.");
                context.Done<object>(null);
            }
        }

        /// <summary>
        /// Method that would run the add dialog
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.AddDialogMatch)]
        [RegexPattern(DialogMatches.AdditionDialogMatch)]
        [RegexPattern(DialogMatches.SumDialogMatch)]
        [ScorableGroup(1)]
        public void RunAddDialog(IDialogContext context, IActivity activity)
        {
            var result = activity as Activity;
            context.Call(new AddDialog(result), this.EndDialog);
        }

        /// <summary>
        /// The method that runs the subtract dialog
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.SubtractDialogMatch)]
        [RegexPattern(DialogMatches.SubtractionDialogMatch)]
        [RegexPattern(DialogMatches.DifferenceDialogMatch)]
        [ScorableGroup(1)]
        public void RunSubtractDialog(IDialogContext context, IActivity activity)
        {
            var result = activity as Activity;
            context.Call(new SubtractDialog(result), this.EndDialog);
        }

        /// <summary>
        /// The method that will run the product dialog
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.ProductDialogMatch)]
        [RegexPattern(DialogMatches.MultiplicationDialogMatch)]
        [RegexPattern(DialogMatches.MultiplyDialogMatch)]
        [ScorableGroup(1)]
        public void RunMultiplyDialog(IDialogContext context, IActivity activity)
        {
            var multiResult = activity as Activity;
            context.Call(new MultiplyDialog(multiResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will calculate the division
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.DivideDialogMatch)]
        [RegexPattern(DialogMatches.DivisionDialogMatch)]
        [RegexPattern(DialogMatches.QuotientDialogMatch)]
        [ScorableGroup(1)]
        public void RunDivideDialog(IDialogContext context, IActivity activity)
        {
            var divideResult = activity as Activity;
            context.Call(new DivideDialog(divideResult), this.EndDialog);
        }

        /// <summary>
        /// Method that fires off the logic to calculator the remainder among a list of numbers
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.RemainderDialogMatch)]
        [RegexPattern(DialogMatches.ModuloDialogMatch)]
        [RegexPattern(DialogMatches.ModulusDialogMatch)]
        [ScorableGroup(1)]
        public void RunModuloDialog(IDialogContext context, IActivity activity)
        {
            var modResult = activity as Activity;
            context.Call(new ModuloDialog(modResult), this.EndDialog);
        }

        /// <summary>
        /// This function calls the dialog to calculate the mean/average
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.AverageDialogMatch)]
        [RegexPattern(DialogMatches.MeanDialogMatch)]
        [ScorableGroup(1)]
        public void RunAverageDialog(IDialogContext context, IActivity activity)
        {
            var averageActivity = activity as Activity;
            context.Call(new AverageDialog(averageActivity), this.EndDialog);
        }

        /// <summary>
        /// This will call the dialog to calculate the median
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.MedianDialogMatch1)]
        [ScorableGroup(1)]
        public void RunMedianDialog(IDialogContext context, IActivity activity)
        {
            var medianActivity = activity as Activity;
            context.Call(new MedianDialog(medianActivity), this.EndDialog);
        }

        /// <summary>
        /// This function will call the dialog to calculate the mode
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.ModeDialogMatch1)]
        [ScorableGroup(1)]
        public void RunModeDialog(IDialogContext context, IActivity activity)
        {
            var modeActivity = activity as Activity;
            context.Call(new ModeDialog(modeActivity), this.EndDialog);
        }

        /// <summary>
        /// The method that will fire off the range calculation
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.RangeDialogMatch1)]
        [ScorableGroup(1)]
        public void RunRangeDialog(IDialogContext context, IActivity activity)
        {
            var rangeActivity = activity as Activity;
            context.Call(new RangeDialog(rangeActivity), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the dialog to calculate the variance among a list of numbers
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.VarianceDialogMatch)]
        [ScorableGroup(1)]
        public void RunVarianceDialog(IDialogContext context, IActivity activity)
        {
            var varianceActivity = activity as Activity;
            context.Call(new VarianceDialog(varianceActivity), this.EndDialog);
        }

        /// <summary>
        /// Method that would fire off the Standard Deviation calculation
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.StandardDeviationDialogMatch1)]
        [ScorableGroup(1)]
        public void RunStandardDeviationDialog(IDialogContext context, IActivity activity)
        {
            var standardDevActivity = activity as Activity;
            context.Call(new StandardDeviationDialog(standardDevActivity), this.EndDialog);
        }

        /// <summary>
        /// Method that would fire off the calculation of the geometric mean
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.GeometricMeanDialogMatch)]
        [ScorableGroup(1)]
        public void RunGeometricMeanDialog(IDialogContext context, IActivity activity)
        {
            var geometricMeanActivity = activity as Activity;
            context.Call(new GeometricMeanDialog(geometricMeanActivity), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the Root Mean Square calculations
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.RmsDialogMatch)]
        [ScorableGroup(1)]
        public void RunRmsDialog(IDialogContext context, IActivity activity)
        {
            var rmsActivity = activity as Activity;
            context.Call(new RmsDialog(rmsActivity), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the hypotenuse given 2 legs
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.PythagorasDialogMatch)]
        [RegexPattern(DialogMatches.PythagoreanDialogMatch)]
        [ScorableGroup(1)]
        public void RunPythagoreanDialog(IDialogContext context, IActivity activity)
        {
            var pythagResult = activity as Activity;
            context.Call(new PythagoreanDialog(pythagResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the discriminant
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.NumberOfRootsDialogMatch)]
        [RegexPattern(DialogMatches.DiscriminantDialogMatch)]
        [ScorableGroup(1)]
        public void RunDiscriminantDialog(IDialogContext context, IActivity activity)
        {
            var discrimResult = activity as Activity;
            context.Call(new DiscriminantDialog(discrimResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the roots of a quadratic equation
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.EquationRootsDialogMatch)]
        [RegexPattern(DialogMatches.QuadraticSolverDialogMatch)]
        [ScorableGroup(1)]
        public void RunQuadraticSolverDialog(IDialogContext context, IActivity activity)
        {
            var quadSolverResult = activity as Activity;
            context.Call(new QuadraticSolverDialog(quadSolverResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the midpoint
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.MidPointDialogMatch)]
        [ScorableGroup(1)]
        public void RunMidpointDialog(IDialogContext context, IActivity activity)
        {
            var midPointResult = activity as Activity;
            context.Call(new MidpointDialog(midPointResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the distance between 2 points
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.DistanceDialogMatch)]
        [ScorableGroup(1)]
        public void RunDistanceDialog(IDialogContext context, IActivity activity)
        {
            var distanceResult = activity as Activity;
            context.Call(new DistanceDialog(distanceResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will calculate the area of a triangle
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.TriangleAreaDialogMatch)]
        [ScorableGroup(1)]
        public void RunTriangleAreaDialog(IDialogContext context, IActivity activity)
        {
            var triangleAreaResult = activity as Activity;
            context.Call(new TriangleAreaDialog(triangleAreaResult), this.EndDialog);
        }

        /// <summary>
        /// Method that fires off the calculation of the perimeter of a triangle
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.TrianglePerimDialogMatch)]
        [ScorableGroup(1)]
        public void RunTrianglePerimDialog(IDialogContext context, IActivity activity)
        {
            var trianglePerimResult = activity as Activity;
            context.Call(new TrianglePerimDialog(trianglePerimResult), this.EndDialog);
        }

        /// <summary>
        /// Method that fires off the calculation of the perimeter of a quadrilateral
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.QuadrilateralPerimDialogMatch)]
        [ScorableGroup(1)]
        public void RunQuadPerimDialog(IDialogContext context, IActivity activity)
        {
            var quadPerimResult = activity as Activity;
            context.Call(new QuadrilateralPerimDialog(quadPerimResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the area of a rectangle
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.RectangleAreaDialogMatch)]
        [ScorableGroup(1)]
        public void RunRectangleAreaDialog(IDialogContext context, IActivity activity)
        {
            var rectangleAreaResult = activity as Activity;
            context.Call(new RectangleAreaDialog(rectangleAreaResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the area of the circle
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.CircleAreaDialogMatch)]
        [ScorableGroup(1)]
        public void RunCircleAreaDialog(IDialogContext context, IActivity activity)
        {
            var circleAreaResult = activity as Activity;
            context.Call(new CircleAreaDialog(circleAreaResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the circumference of a circle
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.CicleCircumferenceDialogMatch)]
        [ScorableGroup(1)]
        public void RunCircumferenceDialog(IDialogContext context, IActivity activity)
        {
            var circumferenceResult = activity as Activity;
            context.Call(new CircumferenceDialog(circumferenceResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the calculation of the area of a trapezoid
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        [RegexPattern(DialogMatches.TrapezoidAreaDialogMatch)]
        [ScorableGroup(1)]
        public void RunTrapezoidAreaDialog(IDialogContext context, IActivity activity)
        {
            var trapezoidAreaResult = activity as Activity;
            context.Call(new TrapezoidAreaDialog(trapezoidAreaResult), this.EndDialog);
        }

        /// <summary>
        /// A default help method that exists as part of practice
        /// </summary>
        /// <param name="context">The current context</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.HelpDialogMatch)]
        [ScorableGroup(1)]
        public async Task GetHelp(IDialogContext context)
        {
            await context.PostAsync(MessageHelpers.CreateHelpMessage(string.Empty));
            context.Done<object>(null);
        }

        /// <summary>
        /// A default method that will be sent out as part of the I don't know
        /// </summary>
        /// <param name="context">The current context</param>
        /// <returns>A unit of execution</returns>
        [MethodBind]
        [ScorableGroup(2)]
        public async Task Default(IDialogContext context)
        {
            // Send message
            await context.PostAsync("I'm sorry, but I didn't understand.");
            context.Done<object>(null);
        }

        /// <summary>
        /// This method would always ensure that the RootDialog would be popped off
        /// the DialogStack whenever the bot is shutting down
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="result">The awaitable object</param>
        /// <returns>A unit of execution</returns>
        public async Task EndDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("What else do you want to do?");
            context.Done<object>(null);
        }
    }
}