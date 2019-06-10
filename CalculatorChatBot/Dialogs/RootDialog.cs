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
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.HelloDialogMatch)]
        [RegexPattern(DialogMatches.HiDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunHelloDialog(IDialogContext context, IActivity activity)
        {
            var helloResult = activity as Activity;
            context.Call(new HelloDialog(helloResult), this.EndDialog);
        }

        /// <summary>
        /// A method that would run the greeting dialog
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.GreetEveryoneDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunGreetDialog(IDialogContext context, IActivity activity)
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
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.AddDialogMatch)]
        [RegexPattern(DialogMatches.AdditionDialogMatch)]
        [RegexPattern(DialogMatches.SumDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunAddDialog(IDialogContext context, IActivity activity)
        {
            var result = activity as Activity;
            context.Call(new AddDialog(result), this.EndDialog);
        }

        /// <summary>
        /// The method that runs the subtract dialog
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.SubtractDialogMatch)]
        [RegexPattern(DialogMatches.SubtractionDialogMatch)]
        [RegexPattern(DialogMatches.DifferenceDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunSubtractDialog(IDialogContext context, IActivity activity)
        {
            var result = activity as Activity;
            context.Call(new SubtractDialog(result), this.EndDialog);
        }

        /// <summary>
        /// The method that will run the product dialog
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.ProductDialogMatch)]
        [RegexPattern(DialogMatches.MultiplicationDialogMatch)]
        [RegexPattern(DialogMatches.MultiplyDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunMultiplyDialog(IDialogContext context, IActivity activity)
        {
            var multiResult = activity as Activity;
            context.Call(new MultiplyDialog(multiResult), this.EndDialog);
        }

        /// <summary>
        /// Method that will calculate the division
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.DivideDialogMatch)]
        [RegexPattern(DialogMatches.DivisionDialogMatch)]
        [RegexPattern(DialogMatches.QuotientDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunDivideDialog(IDialogContext context, IActivity activity)
        {
            var divideResult = activity as Activity;
            context.Call(new DivideDialog(divideResult), this.EndDialog);
        }

        /// <summary>
        /// Method that fires off the logic to calculator the remainder among a list of numbers
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.RemainderDialogMatch)]
        [RegexPattern(DialogMatches.ModuloDialogMatch)]
        [RegexPattern(DialogMatches.ModulusDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunModuloDialog(IDialogContext context, IActivity activity)
        {
            var modResult = activity as Activity;
            context.Call(new ModuloDialog(modResult), this.EndDialog);
        }

        /// <summary>
        /// This function calls the dialog to calculate the mean/average
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.AverageDialogMatch)]
        [RegexPattern(DialogMatches.MeanDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunAverageDialog(IDialogContext context, IActivity activity)
        {
            var averageActivity = activity as Activity;
            context.Call(new AverageDialog(averageActivity), this.EndDialog);
        }

        /// <summary>
        /// This will call the dialog to calculate the median
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.MedianDialogMatch1)]
        [ScorableGroup(1)]
        public async Task RunMedianDialog(IDialogContext context, IActivity activity)
        {
            var medianActivity = activity as Activity;
            context.Call(new MedianDialog(medianActivity), this.EndDialog);
        }

        /// <summary>
        /// This function will call the dialog to calculate the mode
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.ModeDialogMatch1)]
        [ScorableGroup(1)]
        public async Task RunModeDialog(IDialogContext context, IActivity activity)
        {
            var modeActivity = activity as Activity;
            context.Call(new ModeDialog(modeActivity), this.EndDialog);
        }

        /// <summary>
        /// The method that will fire off the range calculation
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.RangeDialogMatch1)]
        [ScorableGroup(1)]
        public async Task RunRangeDialog(IDialogContext context, IActivity activity)
        {
            var rangeActivity = activity as Activity;
            context.Call(new RangeDialog(rangeActivity), this.EndDialog);
        }

        /// <summary>
        /// Method that will fire off the dialog to calculate the variance among a list of numbers
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [RegexPattern(DialogMatches.VarianceDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunVarianceDialog(IDialogContext context, IActivity activity)
        {
            var varianceActivity = activity as Activity;
            context.Call(new VarianceDialog(varianceActivity), this.EndDialog);
        }

        [RegexPattern(DialogMatches.StandardDeviationDialogMatch1)]
        [ScorableGroup(1)]
        public async Task RunStandardDeviationDialog(IDialogContext context, IActivity activity)
        {
            var standardDevActivity = activity as Activity;
            context.Call(new StandardDeviationDialog(standardDevActivity), EndDialog);
        }

        [RegexPattern(DialogMatches.GeometricMeanDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunGeometricMeanDialog(IDialogContext context, IActivity activity)
        {
            var geometricMeanActivity = activity as Activity;
            context.Call(new GeometricMeanDialog(geometricMeanActivity), EndDialog);
        }

        [RegexPattern(DialogMatches.RmsDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunRmsDialog(IDialogContext context, IActivity activity)
        {
            var rmsActivity = activity as Activity;
            context.Call(new RmsDialog(rmsActivity), EndDialog);
        }

        [RegexPattern(DialogMatches.PythagorasDialogMatch)]
        [RegexPattern(DialogMatches.PythagoreanDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunPythagoreanDialog(IDialogContext context, IActivity activity)
        {
            var pythagResult = activity as Activity;
            context.Call(new PythagoreanDialog(pythagResult), EndDialog);
        }

        [RegexPattern(DialogMatches.NumberOfRootsDialogMatch)]
        [RegexPattern(DialogMatches.DiscriminantDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunDiscriminantDialog(IDialogContext context, IActivity activity)
        {
            var discrimResult = activity as Activity;
            context.Call(new DiscriminantDialog(discrimResult), EndDialog);
        }

        [RegexPattern(DialogMatches.EquationRootsDialogMatch)]
        [RegexPattern(DialogMatches.QuadraticSolverDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunQuadraticSolverDialog(IDialogContext context, IActivity activity)
        {
            var quadSolverResult = activity as Activity;
            context.Call(new QuadraticSolverDialog(quadSolverResult), EndDialog);
        }

        [RegexPattern(DialogMatches.MidPointDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunMidpointDialog(IDialogContext context, IActivity activity)
        {
            var midPointResult = activity as Activity;
            context.Call(new MidpointDialog(midPointResult), EndDialog); 
        }

        [RegexPattern(DialogMatches.DistanceDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunDistanceDialog(IDialogContext context, IActivity activity)
        {
            var distanceResult = activity as Activity;
            context.Call(new DistanceDialog(distanceResult), EndDialog); 
        }

        [RegexPattern(DialogMatches.TriangleAreaDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunTriangleAreaDialog(IDialogContext context, IActivity activity)
        {
            var triangleAreaResult = activity as Activity;
            context.Call(new TriangleAreaDialog(triangleAreaResult), EndDialog);
        }

        [RegexPattern(DialogMatches.TrianglePerimDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunTrianglePerimDialog(IDialogContext context, IActivity activity)
        {
            var trianglePerimResult = activity as Activity;
            context.Call(new TrianglePerimDialog(trianglePerimResult), EndDialog);
        }

        [RegexPattern(DialogMatches.QuadrilateralPerimDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunQuadPerimDialog(IDialogContext context, IActivity activity)
        {
            var quadPerimResult = activity as Activity;
            context.Call(new QuadrilateralPerimDialog(quadPerimResult), EndDialog);
        }

        [RegexPattern(DialogMatches.RectangleAreaDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunRectangleAreaDialog(IDialogContext context, IActivity activity)
        {
            var rectangleAreaResult = activity as Activity;
            context.Call(new RectangleAreaDialog(rectangleAreaResult), EndDialog);
        }

        [RegexPattern(DialogMatches.CircleAreaDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunCircleAreaDialog(IDialogContext context, IActivity activity)
        {
            var circleAreaResult = activity as Activity;
            context.Call(new CircleAreaDialog(circleAreaResult), EndDialog);
        }

        [RegexPattern(DialogMatches.CicleCircumferenceDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunCircumferenceDialog(IDialogContext context, IActivity activity)
        {
            var circumferenceResult = activity as Activity;
            context.Call(new CircumferenceDialog(circumferenceResult), EndDialog);
        }

        [RegexPattern(DialogMatches.TrapezoidAreaDialogMatch)]
        [ScorableGroup(1)]
        public async Task RunTrapezoidAreaDialog(IDialogContext context, IActivity activity)
        {
            var trapezoidAreaResult = activity as Activity;
            context.Call(new TrapezoidAreaDialog(trapezoidAreaResult), EndDialog);
        }

        [RegexPattern(DialogMatches.HelpDialogMatch)]
        [ScorableGroup(1)]
        public async Task GetHelp(IDialogContext context, IActivity activity)
        {
            await context.PostAsync(MessageHelpers.CreateHelpMessage(string.Empty));
            context.Done<object>(null);
        }

        /// <summary>
        /// A default method that will be sent out as part of the I don't know
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="activity">The current activity</param>
        /// <returns>A unit of execution</returns>
        [MethodBind]
        [ScorableGroup(2)]
        public async Task Default(IDialogContext context, IActivity activity)
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
            context.Done<object>(null);
        }
    }
}