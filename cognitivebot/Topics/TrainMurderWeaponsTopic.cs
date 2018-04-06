using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cognitivebot.Topics
{
    public class TrainMurderWeaponsTopic : ITopic
    {
        public enum TopicState
        {
            unknown,
            started,
            askedForPicture,
            askedForWeaponName,
            finished
        }

        public TopicState State
        {
            get;
            set;
        } = TopicState.unknown;

        public string Name { get => "TrainTopic"; }

        public async Task<bool> ContinueTopic(DetectiveBotContext context)
        {
            switch (State)
            {
                case TopicState.started:
                    return await AskForPicture(context);
                case TopicState.askedForPicture:
                    return await AskForWeaponName(context);
                case TopicState.askedForWeaponName:
                    return await SaveName(context);
                case TopicState.finished:
                    return await CheckForAnother(context);
                case TopicState.unknown:
                default:
                    return await StartTopic(context);
            }
        }

        private async Task<bool> AskForPicture(DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply("Please send me a picture of the murder weapon");
            await context.SendActivity(reply);
            State = TopicState.askedForPicture;

            return true;
        }

        private async Task<bool> AskForWeaponName(DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply("What is the name of this Weapon?");
            await context.SendActivity(reply);
            State = TopicState.askedForWeaponName;

            return true;
        }

        private async Task<bool> SaveName(DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply($"Storing new suspect {context.Request.Text}");
            await context.SendActivity(reply);


            var reply2 = BotReplies.ReplyWithOptions("Would you like to add another?", new List<string>() { Intents.Yes, Intents.No }, context);
            await context.SendActivity(reply2);
            State = TopicState.finished;

            return true;
        }

        private async Task<bool> CheckForAnother(DetectiveBotContext context)
        {
            if (context.RecognizedIntents.TopIntent?.Name == Intents.Yes)
            {
                State = TopicState.started;
                return await this.ContinueTopic(context);
            }
            else
            {
                return false;
            }

        }



        public async Task<bool> ResumeTopic(DetectiveBotContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StartTopic(DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply("Let's learn something about the murder weapons you find.");
            await context.SendActivity(reply);
            State = TopicState.started;

            return await this.ContinueTopic(context);
        }

    }
}
