// <copyright file="MessageHelpers.cs" company="XYZ Company LLC">
// Copyright (c) XYZ Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.BotHelpers
{
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
        public static async Task SendMessage(IDialogContext context, string message)
        {
            await context.PostAsync(message);
        }

        public static string CreateHelpMessage(string firstLine)
        {
            var sb = new StringBuilder();
            sb.AppendLine(firstLine);
            sb.AppendLine();
            sb.AppendLine("For anything regarding help, navigate to the help tab");
            return sb.ToString();
        }

        public static async Task SendOneToOneWelcomeMessage(
            ConnectorClient connector,
            TeamsChannelData channelData,
            ChannelAccount botAccount,
            ChannelAccount userAccount,
            string tenantId)
        {
            string welcomeMessage = CreateHelpMessage($"The team {channelData.Team.Name} has added the Calculator Chat Bot - helping with some basic math stuff.");
            var response = connector.Conversations.CreateOrGetDirectConversation(botAccount, userAccount, tenantId);
            Activity newActivity = new Activity
            {
                Text = welcomeMessage,
                Type = ActivityTypes.Message,
                Conversation = new ConversationAccount
                {
                    Id = response.Id
                }
            };

            await connector.Conversations.SendToConversationAsync(newActivity);
        }
    }
}