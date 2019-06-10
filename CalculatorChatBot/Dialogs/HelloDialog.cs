namespace CalculatorChatBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    /// <summary>
    /// This class is showing the standard hello world like example. However, the difference is that
    /// the user can provide a number which would tell the bot how many times the bot should reply
    /// with the message 'Hello!'
    /// </summary>
    [Serializable]
    public class HelloDialog : IDialog<object>
    {
        public HelloDialog(Activity incomingActivity)
        {
            // Extract the incoming text
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            // What is the parameter/property to have something done - making sure
            // to look at the length of the incoming string
            this.NumTimes = incomingInfo.Length == 2 ? int.Parse(incomingInfo[1]) : 0;
        }

        public int NumTimes { get; set; }

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
                    await context.PostAsync("Hello!");
                }
            }
            else
            {
                // Send the message 'Hello!' once
                await context.PostAsync("Hello!");
            }

            // Return back to the RootDialog - popping this child dialog off the stack
            context.Done<object>(null);
        }
    }
}