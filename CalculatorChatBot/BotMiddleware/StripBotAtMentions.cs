// <copyright file="StripBotAtMentions.cs" company="XYZ Software Company LLC">
// Copyright (c) XYZ Software Company LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.BotMiddleware
{
    using System;
    using System.Globalization;
    using Microsoft.Bot.Connector;

    /// <summary>
    /// This class is responsible for stripping the AtMentions.
    /// </summary>
    public static class StripBotAtMentions
    {
        /// <summary>
        /// Removes the @mention of the bot to be able to correctly parse the incoming message.
        /// </summary>
        /// <param name="activity">Incoming activity.</param>
        /// <returns>The activity which includes the actual message.</returns>
        public static IMessageActivity StripAtMentionText(IMessageActivity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException(nameof(activity));
            }

            Mention[] m = activity.GetMentions();
            for (int i = 0; i < m.Length; i++)
            {
                if (m[i].Mentioned.Id == activity.Recipient.Id)
                {
                    // Bot is in the @mention list
                    // The below example will strip the bot name out of the message,
                    // so it can be parsed as if the bot name was not included in the message.
                    // Note that the Text object will contain the full bot name, if it is
                    // applicable
                    if (m[i].Text != null)
                    {
                        activity.Text = activity.Text.Replace(m[i].Text, " ").Trim();
                    }
                }
            }

            // Convert the input command in lower case for 1To1 and Channel users
            if (activity.Text != null)
            {
                activity.Text = activity.Text.ToLower(CultureInfo.InvariantCulture);
            }

            return activity;
        }
    }
}