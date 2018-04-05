using System;
using cognitivebot.Topics;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;

namespace cognitivebot
{
    public class DetectiveBotContext : BotContextWrapper
    {
        public DetectiveBotContext(IBotContext context) : base(context)
        {
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
