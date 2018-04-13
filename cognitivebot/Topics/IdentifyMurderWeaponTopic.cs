using cognitivebot.Services;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace cognitivebot.Topics
{
    public class IdentifyMurderWeaponTopic : ITopic
    {
        public IdentifyMurderWeaponTopic()
        {
        }

        public string Name { get => "IdentifyTopic"; }

        public async Task<bool> ContinueTopic(DetectiveBotContext context)
        {
            if (context.Request.Attachments != null && context.Request.Attachments.Count > 0)
            {
                CustomVisionService customVisionService = new CustomVisionService();

                var result = await customVisionService.IdentifyWeapon(context.Request.Attachments[0].ContentUrl);

                if (result != null)
                {
                    var maxResult = result.Predictions.OrderByDescending(c => c.Probability).First();
                                        
                    var resultReply = context.Request.CreateReply($"I think it is {maxResult.Tag} with probability {maxResult.Probability:P1}");

                    await context.SendActivity(resultReply);
                }
                else
                {
                    var resultReply = context.Request.CreateReply($"I'm not sure what this is");
                    await context.SendActivity(resultReply);
                }
            }

            var reply = context.Request.CreateReply("Please send me a picture to identify the next weapon or type \"Quit\" to stop");
            await context.SendActivity(reply);

            return true;
        }

        public async Task<bool> ResumeTopic(DetectiveBotContext context)
        {
            return await this.ContinueTopic(context);
        }

        public async Task<bool> StartTopic(DetectiveBotContext context)
        {
            return await this.ContinueTopic(context);
        }
    }
}
