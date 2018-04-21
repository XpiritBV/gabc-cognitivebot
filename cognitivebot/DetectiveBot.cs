using System;
using System.Linq;
using System.Threading.Tasks;
using cognitivebot.Services;
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
        public ILogger _logger { get; set; }
        public IFaceRecognitionService _faceRecognitionService { get; set; }
        public ICustomVisionService _customVisionService { get; set; }

        public DetectiveBot(ILoggerFactory loggerFactory, ICustomVisionService customVisionService, IFaceRecognitionService faceRecognitionService)
        {
            _logger = loggerFactory?.CreateLogger(GetType().Name);
            _faceRecognitionService = faceRecognitionService;
            _customVisionService = customVisionService;
        }

        /// <inheritdoc />
        public async Task OnReceiveActivity(IBotContext context)
        {
            var botContext = new DetectiveBotContext(context, _faceRecognitionService, _customVisionService);

            switch (botContext.Request.Type)
            {
                case ActivityTypes.ConversationUpdate:
                    await GetOrAddUserProfile(botContext);
                    break;
                case ActivityTypes.Message:
                    await HandleMessage(botContext);
                    break;
            }
        }

        public async Task HandleMessage(IBotContext botContext)
        {
            var context = new DetectiveBotContext(botContext, _faceRecognitionService, _customVisionService);

            var handled = false;

            if (context.RecognizedIntents.TopIntent?.Name == Intents.Quit)
            {
                await context.SendActivity($"Stopping current action");
                context.ConversationState.ActiveTopic = new DefaultTopic(_customVisionService, _faceRecognitionService);
                handled = await context.ConversationState.ActiveTopic.StartTopic(context);
                return;
            }

            if(context.ConversationState.ActiveTopic == null)
            {
                context.ConversationState.ActiveTopic = new DefaultTopic(_customVisionService, _faceRecognitionService);
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
                context.ConversationState.ActiveTopic = new DefaultTopic(_customVisionService, _faceRecognitionService);
                handled = await context.ConversationState.ActiveTopic.StartTopic(context);
            }
        }

        private async Task GetOrAddUserProfile(IBotContext botContext)
        {
            try
            {
                var context = new DetectiveBotContext(botContext, _faceRecognitionService, _customVisionService);

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
