// <copyright file="GreetDialog.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Connector.Teams.Models;

    /// <summary>
    /// This is the dialog to greet people.
    /// </summary>
    [Serializable]
    public class GreetDialog : IDialog<object>
    {
        /// <summary>
        /// This method will run whenever a dialog is being invoked.
        /// </summary>
        /// <param name="context">The current dialog context.</param>
        /// <returns>A unit of execution.</returns>
        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Creating the connector client which will be used to make requests
            // for roster and conversation data.
            using (var connectorClient = new ConnectorClient(new Uri(context.Activity.ServiceUrl)))
            {
                // Fetching the members of the conversation and data from the current conversation
                var members = await connectorClient.Conversations.GetConversationMembersAsync(context.Activity.Conversation.Id).ConfigureAwait(false);
                var channelData = context.Activity.GetChannelData<TeamsChannelData>();

                // The bot is obviously the recipient of any and all messages that are coming in
                var bot = context.Activity.Recipient;

                // Iterating through the members in the team roster
                foreach (var member in members)
                {
                    if (member.Id == context.Activity.From.Id)
                    {
                        Debug.WriteLine("This is the user that initiated the greeting to everyone else.");
                        continue;
                    }

                    // Fetch and increment the number of times that the user has been greeted.
                    if (!context.ConversationData.TryGetValue(member.Id, out int timesGreeted))
                    {
                        timesGreeted = 0;
                    }

                    timesGreeted++;
                    context.ConversationData.SetValue(member.Id, timesGreeted);

                    // Building the 1:1 conversation parameters
                    var parameters = new ConversationParameters
                    {
                        Bot = bot,
                        Members = new ChannelAccount[] { member },
                        ChannelData = new TeamsChannelData
                        {
                            Tenant = channelData.Tenant,
                        },
                    };

                    // Create the conversation. If the bot has never talked to the user before this conversation
                    // will not exist; this ensures that the conversation in fact does exist. - so therefore a new
                    // conversation may get created
                    var conversationResource = await connectorClient.Conversations.CreateConversationAsync(parameters).ConfigureAwait(false);

                    // Create and now send the response message.
                    var message = Activity.CreateMessageActivity();
                    message.From = bot;
                    message.Conversation = new ConversationAccount(id: conversationResource.Id);
                    message.Text = "Greetings! (I have greeted you " + timesGreeted + " time" + (timesGreeted != 1 ? "s" : string.Empty) + ")";

                    await connectorClient.Conversations.SendToConversationAsync((Activity)message).ConfigureAwait(false);
                }
            }

            context.Done<object>(null);
        }
    }
}