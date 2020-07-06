// <copyright file="WelcomeTeamAdaptiveCard.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web.Hosting;
    using CalculatorChatBot.Properties;
    using Microsoft.Azure;

    /// <summary>
    /// This class is for the welcome team adaptive card.
    /// </summary>
    public static class WelcomeTeamAdaptiveCard
    {
        private static readonly string CardTemplate;

        /// <summary>
        /// Initializes static members of the <see cref="WelcomeTeamAdaptiveCard"/> class.
        /// </summary>
        static WelcomeTeamAdaptiveCard()
        {
            var cardJsonFilePath = HostingEnvironment.MapPath("~/Cards/WelcomeTeamAdaptiveCard.json");
            CardTemplate = File.ReadAllText(cardJsonFilePath);
        }

        /// <summary>
        /// This method constructs the JSON string of the card.
        /// </summary>
        /// <param name="teamName">The team name.</param>
        /// <param name="botDisplayName">The bot display name.</param>
        /// <returns>The card JSON string.</returns>
        public static string GetCard(string teamName, string botDisplayName)
        {
            var welcomeTeamCardTitleText = Resources.WelcomeTeamCardTitleText;
            var welcomeTeamCardIntroPart1 = string.Format(CultureInfo.InvariantCulture, Resources.WelcomeTeamCardIntroPart1, botDisplayName, teamName);
            var welcomeTeamCardIntroPart2 = Resources.WelcomeTeamCardIntroPart2;
            var tourButtonText = Resources.TourButtonText;
            var welcomeTourTitle = Resources.WelcomeTourTitle;

            var baseDomain = CloudConfigurationManager.GetSetting("AppBaseDomain");
            var htmlUrl = Uri.EscapeDataString($"https:{baseDomain}/Content/tour.html?theme={{theme}}");
            var manifestAppId = CloudConfigurationManager.GetSetting("ManifestAppId");
            var tourUrl = $"https://teams.microsoft.com/l/task/{manifestAppId}?url={htmlUrl}&height=533px&width=600px&title={welcomeTourTitle}";

            var variablesToValues = new Dictionary<string, string>()
            {
                { "welcomeTeamCardTitleText", welcomeTeamCardTitleText },
                { "welcomeTeamCardIntroPart1", welcomeTeamCardIntroPart1 },
                { "welcomeTeamCardIntroPart2", welcomeTeamCardIntroPart2 },
                { "tourButtonText", tourButtonText },
                { "tourUrl", tourUrl },
            };

            var cardBody = CardTemplate;
            foreach (var kvp in variablesToValues)
            {
                cardBody = cardBody.Replace($"%{kvp.Key}%", kvp.Value);
            }

            return cardBody;
        }
    }
}