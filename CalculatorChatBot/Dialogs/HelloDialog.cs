// <copyright file="HelloDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    /// <summary>
    /// This class is showing the standard hello world like example. However, the difference is that
    /// the user can provide a number which would tell the bot how many times the bot should reply
    /// with the message saying hello.
    /// </summary>
    [Serializable]
    public class HelloDialog : IDialog<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelloDialog"/> class.
        /// </summary>
        /// <param name="incomingActivity">The incoming activity.</param>
        public HelloDialog(Activity incomingActivity)
        {
            if (incomingActivity is null)
            {
                throw new ArgumentNullException(nameof(incomingActivity));
            }

            // Extract the incoming text
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            // What is the parameter/property to have something done - making sure
            // to look at the length of the incoming string
            this.NumTimes = incomingInfo.Length == 2 ? int.Parse(incomingInfo[1], CultureInfo.InvariantCulture) : 0;
        }

        /// <summary>
        /// Gets or sets the number of times.
        /// </summary>
        public int NumTimes { get; set; }

        /// <summary>
        /// This method will be executing when this dialog is running.
        /// </summary>
        /// <param name="context">The dialog context.</param>
        /// <returns>A unit of execution.</returns>
        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (this.NumTimes > 0)
            {
                for (int i = 0; i < this.NumTimes; i++)
                {
                    // Sending out the standard greeting message
                    await context.PostAsync("Hello!").ConfigureAwait(false);
                }
            }
            else
            {
                await context.PostAsync("Hello!").ConfigureAwait(false);
            }

            context.Done<object>(null);
        }
    }
}