using System;
using cognitivebot.Services;
using cognitivebot.Topics;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;

namespace cognitivebot
{
    public class DetectiveBotContext : BotContextWrapper
    {
        private IFaceRecognitionService _faceRecognitionService;
        private ICustomVisionService _customVisionService;

        public DetectiveBotContext(IBotContext context, IFaceRecognitionService faceRecognitionService, ICustomVisionService customVisionService) : base(context)
        {
            _faceRecognitionService = faceRecognitionService;
            _customVisionService = customVisionService;
        }

        public IRecognizedIntents RecognizedIntents { get { return this.Get<IRecognizedIntents>(); } }

        public ConversationData ConversationState
        {
            get
            {
                return ConversationState<ConversationData>.Get(this);
            }
        }

        public UserData UserState
        {
            get
            {
                return UserState<UserData>.Get(this);
            }
        }
    }

    public class ConversationData : StoreItem
    {
        public ITopic ActiveTopic { get; set; }
    }

    /// <summary>
    /// Object persisted as user state
    /// </summary>
    public class UserData : StoreItem
    {
    }
}
