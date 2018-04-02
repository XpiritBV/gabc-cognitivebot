using System;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public DetectiveBot(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger(GetType().Name);
        }

        /// <inheritdoc />
        public async Task OnReceiveActivity(IBotContext context)
        {
            var state = context.GetConversationState<Topic>();
            switch (context.Request.Type)
            {
                case ActivityTypes.ConversationUpdate:
                    await GetOrAddUserProfile(context);
                    break;
                case ActivityTypes.Message:
                    var luisResult = context.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);
                    //dialog
                    if (luisResult != null)
                    {
                        //await ExecuteLuisIntent(context, luisResult);
                    }
                    //response
                    else if (context.Request.Value is JToken token)
                    {
                        var submittedData = token.ToObject<SubmittedData>();
                        var topic = state.ActiveTopic ?? Activities.None;
                        string action = submittedData.Action;


                        switch (action)
                        {
                            case Activities.TrainSuspects:
                                break;
                            case Activities.CheckSuspects:
                                break;
                            case Activities.GetFaceAttributes:
                                break;
                        }
                    }

                    break;
            }
        }

        private async Task GetOrAddUserProfile(IBotContext context)
        {
            try
            {
                var conversationState = context.GetConversationState<Topic>();
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


    public class SubmittedData
    {
        public string Action { get; set; }
        public string Type { get; set; }
        [JsonProperty("undefined")]
        public string Payload { get; set; }
    }
}
