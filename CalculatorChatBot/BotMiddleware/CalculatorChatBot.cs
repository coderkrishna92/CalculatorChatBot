// <copyright file="CalculatorChatBot.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CalculatorChatBot.BotMiddleware
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Cards;
    using Microsoft.Azure;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Connector.Teams;
    using Newtonsoft.Json;

    public class CalculatorChatBot
    {
        /// <summary>
        /// Method to welcome the team - sending the message in the general channel
        /// </summary>
        /// <param name="connectorClient">The connectorClient</param>
        /// <param name="activity">Incoming activity</param>
        /// <param name="tenantId">The tenantId</param>
        /// <param name="teamId">The teamId</param>
        /// <returns>A unit of execution</returns>
        public static async Task WelcomeTeam(ConnectorClient connectorClient, Activity activity, string tenantId, string teamId)
        {
            var botDisplayName = CloudConfigurationManager.GetSetting("BotDisplayName");
            var teamName = await GetTeamNameAsync(connectorClient, teamId);
            var welcomeTeamMessageCard = WelcomeTeamAdaptiveCard.GetCard(teamName, botDisplayName);
            await NotifyTeam(connectorClient, welcomeTeamMessageCard, teamId);
        }

        public static async Task WelcomeUser(ConnectorClient connectorClient, string memberAddedId, string tenantId, string teamId, string botDisplayName)
        {
            var teamName = await GetTeamNameAsync(connectorClient, teamId);
            var allMembers = await connectorClient.Conversations.GetConversationMembersAsync(teamId);

            ChannelAccount userThatJustJoined = null;
            foreach (var m in allMembers)
            {
                if (m.Id == memberAddedId)
                {
                    userThatJustJoined = m;
                    break;
                }
            }

            if (userThatJustJoined != null)
            {
                var welcomeUserMessageCard = WelcomeUserAdaptiveCard.GetCard(teamName, userThatJustJoined.Name, botDisplayName);
                await NotifyUser(connectorClient, welcomeUserMessageCard, userThatJustJoined, tenantId);
            }
        }

        private static async Task<string> GetTeamNameAsync(ConnectorClient connectorClient, string teamId)
        {
            var teamsConnectorClient = connectorClient.GetTeamsConnectorClient();
            var teamDetailsResult = await teamsConnectorClient.Teams.FetchTeamDetailsAsync(teamId);
            return teamDetailsResult.Name;
        }

        private static async Task NotifyTeam(ConnectorClient connectorClient, string cardToSend, string teamId)
        {
            try
            {
                var welcomeTeamReplyActivity = new Activity()
                {
                    Type = ActivityTypes.Message,
                    Conversation = new ConversationAccount()
                    {
                        Id = teamId
                    },
                    Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = "application/vnd.microsoft.card.adaptive",
                            Content = JsonConvert.DeserializeObject(cardToSend)
                        }
                    }
                };

                await connectorClient.Conversations.SendToConversationAsync(welcomeTeamReplyActivity);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"I have hit a snag: {ex.InnerException.ToString()}");
            }
        }

        private static async Task NotifyUser(ConnectorClient connectorClient, string cardToSend, ChannelAccount user, string tenantId)
        {
            try
            {
                var bot = new ChannelAccount { Id = CloudConfigurationManager.GetSetting("MicrosoftAppId") };
                var response = connectorClient.Conversations.CreateOrGetDirectConversation(bot, user, tenantId);

                // Construct the message that we would like to post
                var activity = new Activity()
                {
                    Type = ActivityTypes.Message,
                    Conversation = new ConversationAccount()
                    {
                        Id = response.Id
                    }, 
                    Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = "application/vnd.microsoft.card.adaptive",
                            Content = JsonConvert.DeserializeObject(cardToSend)
                        }
                    }
                };

                await connectorClient.Conversations.SendToConversationAsync(activity);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"I have hit a snag: {ex.InnerException.ToString()}");
            }
        }
    }
}