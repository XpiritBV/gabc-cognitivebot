using System;
using System.Linq;
using System.Threading.Tasks;
using cognitivebot.Topics;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.LUIS;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cognitivebot
{
    public class DetectiveBot : IBot
    {
        private ILogger _logger;

        public DetectiveBot(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger(GetType().Name);
        }

        /// <inheritdoc />
        public async Task OnReceiveActivity(IBotContext context)
        {
            switch (context.Request.Type)
            {
                case ActivityTypes.ConversationUpdate:
                    await GetOrAddUserProfile(context);
                    break;
                case ActivityTypes.Message:
                    await HandleMessage(context);
                    break;
            }
        }

        public async Task HandleMessage(IBotContext botContext)
        {
            var context = new DetectiveBotContext(botContext);

            var handled = false;

            if(context.ConversationState.ActiveTopic == null)
            {
                context.ConversationState.ActiveTopic = new DefaultTopic();
                handled = await context.ConversationState.ActiveTopic.StartTopic(context);
            }
            else
            {
                handled = await context.ConversationState.ActiveTopic.ContinueTopic(context);
            }

            // if activeTopic's result is false and the activeTopic is NOT already the default topic
            if (handled == false && !(context.ConversationState.ActiveTopic is DefaultTopic))
            {
                // USe DefaultTopic as the active topic
                context.ConversationState.ActiveTopic = new DefaultTopic();
                handled = await context.ConversationState.ActiveTopic.ResumeTopic(context);
            }
        }

        private async Task GetOrAddUserProfile(IBotContext botContext)
        {
            try
            {
                var context = new DetectiveBotContext(botContext);

                var activity = context.Request.AsConversationUpdateActivity();
                var user = activity.MembersAdded.Where(m => m.Id == activity.Recipient.Id).FirstOrDefault();
                if (user != null)
                {
                    await context.SendActivity($"Hello {user.Name}, welcome to the Detective bot!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user.");
            }
        }

    }
}
