using System;
using System.Threading.Tasks;

namespace cognitivebot.Topics
{
    public class TrainSuspectTopic : ITopic
    {
        public TrainSuspectTopic()
        {
        }

        public string Name { get => ""; }

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
