using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdaptiveCards;
using cognitivebot.Services;
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
            switch(context.RecognizedIntents.TopIntent?.Name)
            {
                case Intents.Train:
                    context.ConversationState.ActiveTopic = new TrainTopic();
                    return await context.ConversationState.ActiveTopic.StartTopic(context);
                case Intents.IdentifySuspect:
                    context.ConversationState.ActiveTopic = new IdentifySuspectTopic();
                    return await context.ConversationState.ActiveTopic.StartTopic(context);
                case Intents.IdentifyMurderWeapon:
                    context.ConversationState.ActiveTopic = new IdentifyMurderWeaponTopic();
                    return await context.ConversationState.ActiveTopic.StartTopic(context);
                case Intents.MatchSuspect:
                    context.ConversationState.ActiveTopic = new MatchSuspectTopic(new FaceRecognitionService());
                    return await context.ConversationState.ActiveTopic.StartTopic(context);
                default:
                    var reply3 = context.Request.CreateReply("Sorry i can't help you with that");
                    await context.SendActivity(reply3);
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
            var reply = BotReplies.ReplyWithOptions("How may i help you?", new List<string>() {Intents.Train, Intents.IdentifySuspect, Intents.IdentifyMurderWeapon, Intents.MatchSuspect}, context);
            await context.SendActivity(reply);
            State = TopicState.askedTopic;

            return true;
        }


    }
}
