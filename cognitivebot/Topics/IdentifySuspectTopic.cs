using System;
using System.Threading.Tasks;
using cognitivebot.Services;

namespace cognitivebot.Topics
{
    public class IdentifySuspectTopic : ITopic
    {
        public IdentifySuspectTopic()
        {
        }

        public string Name { get => "IdentifyTopic"; }

        public async Task<bool> ContinueTopic(DetectiveBotContext context)
        {
            if (context.Request.Attachments != null && context.Request.Attachments.Count > 0)
            {
                FaceRecognitionService faceRecognitionService = new FaceRecognitionService();
                var result = await faceRecognitionService.IdentifyPerson(context.Request.Attachments[0].ContentUrl);

                if(result != null)
                {
                    var resultReply = context.Request.CreateReply($"I think it is {result.Name}");
                    await context.SendActivity(resultReply);  
                }
                else
                {
                    var resultReply = context.Request.CreateReply($"I'm not sure who this is");
                    await context.SendActivity(resultReply);   
                }
            }

            var reply = context.Request.CreateReply("Please send me a picture to identify the next person or type \"Quit\" to stop");
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
