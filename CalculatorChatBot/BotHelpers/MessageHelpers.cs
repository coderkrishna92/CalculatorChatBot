// <copyright file="MessageHelpers.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.BotHelpers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Connector.Teams.Models;

    /// <summary>
    /// This class contains various helper methods.
    /// </summary>
    public static class MessageHelpers
    {
        /// <summary>
        /// This method will send the message to the end-user.
        /// </summary>
        /// <param name="context">The current dialog being executed.</param>
        /// <param name="message">The message to send.</param>
        /// <returns>A unit of execution.</returns>
        public static async Task SendMessage(IDialogContext context, string message)
        {
            await context.PostAsync(message).ConfigureAwait(false);
        }

        /// <summary>
        /// This method will create a help message to be sent to the end-user.
        /// </summary>
        /// <param name="firstLine">The first line of the help message.</param>
        /// <returns>A string that is concatenated through the StringBuilder class.</returns>
        public static string CreateHelpMessage(string firstLine)
        {
            var sb = new StringBuilder();
            sb.AppendLine(firstLine);
            sb.AppendLine();
            sb.AppendLine("For anything regarding help, navigate to the help tab");
            return sb.ToString();
        }

        /// <summary>
        /// The method that will send the 1:1 welcome message.
        /// </summary>
        /// <param name="connector">The connector client.</param>
        /// <param name="channelData">The Teams Channel Data object.</param>
        /// <param name="botAccount">The bot itself.</param>
        /// <param name="userAccount">The user account.</param>
        /// <param name="tenantId">The Microsoft Teams tenant ID.</param>
        /// <returns>A unit of execution.</returns>
        public static async Task SendOneToOneWelcomeMessage(
            ConnectorClient connector,
            TeamsChannelData channelData,
            ChannelAccount botAccount,
            ChannelAccount userAccount,
            string tenantId)
        {
            if (channelData is null)
            {
                throw new ArgumentNullException(nameof(channelData));
            }

            if (connector is null)
            {
                throw new ArgumentNullException(nameof(connector));
            }

            string welcomeMessage = CreateHelpMessage($"The team {channelData.Team.Name} has added the Calculator Chat Bot - helping with some basic math stuff.");
            var response = connector.Conversations.CreateOrGetDirectConversation(botAccount, userAccount, tenantId);
            Activity newActivity = new Activity
            {
                Text = welcomeMessage,
                Type = ActivityTypes.Message,
                Conversation = new ConversationAccount
                {
                    Id = response.Id,
                },
            };

            await connector.Conversations.SendToConversationAsync(newActivity).ConfigureAwait(false);
        }
    }
}