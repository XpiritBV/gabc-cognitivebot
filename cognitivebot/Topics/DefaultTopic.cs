using System;
using System.Threading.Tasks;

namespace cognitivebot.Topics
{
    public class DefaultTopic : ITopic
    {
        public DefaultTopic()
        {
        }

        public string Name { get => "Default"; }

        public async Task<bool> ContinueTopic(DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply("Continue default");
            await context.SendActivity(reply);
            return true;     
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
            await context.SendActivity(reply);
            return true;
        }
    }
}
