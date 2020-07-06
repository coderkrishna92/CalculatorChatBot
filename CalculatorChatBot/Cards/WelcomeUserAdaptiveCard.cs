// <copyright file="WelcomeUserAdaptiveCard.cs" company="XYZ Software Company LLC">
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
    /// This class is the adaptive card for welcoming the user.
    /// </summary>
    public static class WelcomeUserAdaptiveCard
    {
        private static readonly string CardTemplate;

        static WelcomeUserAdaptiveCard()
        {
            var cardJsonFilePath = HostingEnvironment.MapPath("~/Cards/WelcomeUserAdaptiveCard.json");
            CardTemplate = File.ReadAllText(cardJsonFilePath);
        }

        /// <summary>
        /// This method will construct the card JSON string.
        /// </summary>
        /// <param name="teamName">The name of the team.</param>
        /// <param name="nameOfUserThatJustJoined">The name of the user to welcome.</param>
        /// <param name="botDisplayName">The bot display name.</param>
        /// <returns>The JSON string of the adaptive card.</returns>
        public static string GetCard(string teamName, string nameOfUserThatJustJoined, string botDisplayName)
        {
            var welcomeUserCardTitleText = Resources.WelcomeUserCardTitleText;
            var welcomeUserCardIntroPart1 = string.Format(CultureInfo.InvariantCulture, Resources.WelcomeUserCardIntroPart1, nameOfUserThatJustJoined, teamName, botDisplayName);
            var welcomeUserCardIntroPart2 = Resources.WelcomeUserCardIntroPart2;
            var tourButtonText = Resources.TourButtonText;
            var welcomeTourTitle = Resources.WelcomeTourTitle;

            var baseDomain = CloudConfigurationManager.GetSetting("AppBaseDomain");
            var htmlUrl = Uri.EscapeDataString($"https:{baseDomain}/Content/tour.html?theme={{theme}}");
            var manifestAppId = CloudConfigurationManager.GetSetting("ManifestAppId");
            var tourUrl = $"https://teams.microsoft.com/l/task/{manifestAppId}?url={htmlUrl}&height=533px&width=600px&title={welcomeTourTitle}";

            var variablesToValues = new Dictionary<string, string>()
            {
                { "welcomeUserCardTitleText", welcomeUserCardTitleText },
                { "welcomeUserCardIntroPart1", welcomeUserCardIntroPart1 },
                { "welcomeUserCardIntroPart2", welcomeUserCardIntroPart2 },
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