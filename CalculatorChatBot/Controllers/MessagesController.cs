// <copyright file="MessagesController.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using CalculatorChatBot.BotMiddleware;
    using CalculatorChatBot.Dialogs;
    using Microsoft.Azure;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Connector.Teams.Models;

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// <returns> A unit of execution </returns>
        /// <param name="activity">The incoming activity</param>
        [HttpPost]
        [Route("api/messages")]
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            using (var connectorClient = new ConnectorClient(new Uri(activity.ServiceUrl)))
            {
                if (activity.Type == ActivityTypes.Message)
                {
                    await this.HandleTextMessageAsync(connectorClient, activity);
                }
                else
                {
                    await this.HandleSystemMessageAsync(connectorClient, activity);
                }
            }

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// This is called each time there is a text message being sent to the bot
        /// </summary>
        /// <param name="client">The connectorClient</param>
        /// <param name="activity">The incoming activity</param>
        /// <returns>A unit of execution</returns>
        private async Task HandleTextMessageAsync(ConnectorClient client, Activity activity)
        {
            // This is used for removing the '@botName' from the incoming message so it
            // can be parsed correctly
            var messageActivity = activity.Conversation.ConversationType == "personal" ? activity : 
                                  StripBotAtMentions.StripAtMentionText(activity);
            try
            {
                // This sends all messages to the RootDialog for processing.
                await Conversation.SendAsync(messageActivity, () => new RootDialog());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// This method handles system activity
        /// </summary>
        /// <param name="connectorClient">The connectorClient</param>
        /// <param name="message">The message that is coming in</param>
        /// <returns>A unit of execution</returns>
        private async Task HandleSystemMessageAsync(ConnectorClient connectorClient, Activity message)
        {
            try
            {
                var teamsChannelData = message.GetChannelData<TeamsChannelData>();
                var tenantId = teamsChannelData.Tenant.Id;
                var botDisplayName = CloudConfigurationManager.GetSetting("BotDisplayName");

                if (message.Type == ActivityTypes.ConversationUpdate)
                {
                    // Looking at this from the teams perspective, as opposed to 1:1
                    if (string.IsNullOrEmpty(teamsChannelData?.Team?.Id))
                    {
                        // conversationUpdate really applies to the 1:1 chat
                        return;
                    }

                    string myBotId = message.Recipient.Id;
                    string teamId = message.Conversation.Id;

                    if (message.MembersAdded?.Count > 0)
                    {
                        foreach (var member in message.MembersAdded)
                        {
                            if (member.Id == myBotId)
                            {
                                await CalculatorChatBot.WelcomeTeam(connectorClient, message, tenantId, teamId);
                            }
                            else
                            {
                                await CalculatorChatBot.WelcomeUser(connectorClient, member.Id, tenantId, teamId, botDisplayName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception has been hit: {ex.InnerException.ToString()}");
            }
        }
    }
}