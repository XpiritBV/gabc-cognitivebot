using System;
using System.Threading.Tasks;

namespace cognitivebot.Topics
{
    public class IdentifyMurderWeaponTopic : ITopic
    {
        public IdentifyMurderWeaponTopic()
        {
        }

        public string Name { get => "IdentifyTopic"; }

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
