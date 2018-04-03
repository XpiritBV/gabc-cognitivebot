using System;
using System.Threading.Tasks;

namespace cognitivebot.Topics
{
    public class DefaultTopic : ITopic
    {
        public DefaultTopic()
        {
        }

        public string Name { get => Activities.Default; }

        public Task<bool> ContinueTopic(DetectiveBotContext context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResumeTopic(DetectiveBotContext context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartTopic(DetectiveBotContext context)
        {
            throw new NotImplementedException();
        }
    }
}
