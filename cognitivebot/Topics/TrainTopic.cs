using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cognitivebot.Topics
{
    public class TrainTopic : ITopic
    {
        public enum TopicState
        {
            unknown,
            askedTopic
        }

        public TopicState State
        {
            get;
            set;
        } = TopicState.unknown;


        public string Name { get => "TrainTopic"; }

        public async Task<bool> ContinueTopic(DetectiveBotContext context)
        {
            switch(State)
            {
                case TopicState.askedTopic:
                    return await HandleSelectedTopic(context);
                case TopicState.unknown:
                default:
                    return await StartTopic(context);
            } 
        }

        private async Task<bool> HandleSelectedTopic(DetectiveBotContext context)
        {
            State = TopicState.unknown;
            switch (context.RecognizedIntents.TopIntent?.Name)
            {
                case Intents.Suspects:
                    context.ConversationState.ActiveTopic = new TrainSuspectsTopic();
                    return await context.ConversationState.ActiveTopic.StartTopic(context);
                default:
                    var reply = context.Request.CreateReply("Sorry i can't help you with that");
                    await context.SendActivity(reply);
                    return false;
            }
        }


        public async Task<bool> ResumeTopic(DetectiveBotContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StartTopic(DetectiveBotContext context)
        {
            var reply = BotReplies.ReplyWithOptions("What would you like to train?", new List<string>() { Intents.Suspects }, context);
            await context.SendActivity(reply);
            State = TopicState.askedTopic;

            return true;
        }
    }
}
