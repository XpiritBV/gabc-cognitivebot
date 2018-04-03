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

        public async Task HandleMessage(IBotContext context)
        {
            var state = context.GetConversationState<DetectiveBotContext>();

            var handled = false;

            if(state.ActiveTopic == null)
            {
                state.ActiveTopic = new DefaultTopic();
                handled = await state.ActiveTopic.StartTopic(state);
            }
            else
            {
                handled = await state.ActiveTopic.ContinueTopic(state);
            }

            // if activeTopic's result is false and the activeTopic is NOT already the default topic
            if (handled == false && !(state.ActiveTopic is DefaultTopic))
            {
                // USe DefaultTopic as the active topic
                state.ActiveTopic = new DefaultTopic();
                handled = await state.ActiveTopic.ResumeTopic(state);
            }


            //switch (state.ActiveTopic.Name)
            //{
            //    case Activities.TrainSuspects:
            //        break;
            //    case Activities.GetFaceAttributes:
            //        break;
            //    case Activities.CheckSuspects:
            //        break;
            //    default:
                    
            //        break;
            //}
        }

        private async Task GetOrAddUserProfile(IBotContext context)
        {
            try
            {
                var conversationState = context.GetConversationState<DetectiveBotContext>();
                var userState = context.GetUserState<UserStateModel>();

                //new conversation
                if (conversationState.ActiveUserId == null)
                {
                    var newMember = context.Request.MembersAdded.SingleOrDefault(m => m.Name != "Bot");
                    if (newMember != null)
                    {
                        await context.SendActivity($"Hello {newMember.Name}, welcome to the Detective bot!");
                        conversationState.ActiveUserId = newMember.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user profile.");
            }
        }

        private static string GetEntityValue(RecognizerResult luisResult, string propertyName)
        {
            return ((JValue)luisResult.Entities.GetValue(propertyName)?.Single())?.Value?.ToString();
        }
    }
}
