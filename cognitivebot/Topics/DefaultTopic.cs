using System;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;

namespace cognitivebot.Topics
{
    public class DefaultTopic : ITopic
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

        public string Name { get => "Default"; }

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
            var topic = context.Request.Text.Trim().ToLower();
            switch(topic)
            {
                case "train faces":
                    var reply = context.Request.CreateReply("Chosen Train Faces");
                    await context.SendActivity(reply);
                    return true;
                case "get suspect description":
                    var reply2 = context.Request.CreateReply("Get Suspect description");
                    await context.SendActivity(reply2);
                    return true;
                case "check suspect picture":
                    var reply3 = context.Request.CreateReply("Check suspect picture");
                    await context.SendActivity(reply3);
                    return true;
                default:
                    return false;
            }
        }

        public async Task<bool> ResumeTopic(DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply("Welcome back? How may i help you?");
            await context.SendActivity(reply);
            return true;
        }

        public async Task<bool> StartTopic(DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply("I'm a super smart detective, which of my services do you request?");

            reply.SuggestedActions = new SuggestedActions(
                actions:
                (new CardAction[] {
                new CardAction(type: ActionTypes.ImBack, title: "Train Faces", value: "Train Faces"),
                new CardAction(type: ActionTypes.ImBack, title: "Get Suspect description", value: "Get Suspect description"),
                new CardAction(type: ActionTypes.ImBack, title: "Check suspect picture", value: "Check suspect picture")
            }));

            await context.SendActivity(reply);
            State = TopicState.askedTopic;

            return true;
        }


    }
}
