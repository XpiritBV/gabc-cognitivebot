using System;
using System.Threading.Tasks;
using cognitivebot.Services;

namespace cognitivebot.Topics
{
    public class DescribePersonTopic : ITopic
    {
        IFaceRecognitionService faceRecognitionService { get; set; }

        public DescribePersonTopic(IFaceRecognitionService faceRecognitionService)
        {
            this.faceRecognitionService = faceRecognitionService;
        }

        public string Name { get => "DescribePersonTopic"; }

        public async Task<bool> ContinueTopic(DetectiveBotContext context)
        {
            if (context.Request.Attachments != null && context.Request.Attachments.Count > 0)
            {
                var result = await faceRecognitionService.GetFaceAttributes(context.Request.Attachments[0].ContentUrl);
               
                if(result != null)
                {
                    var resultReply = context.Request.CreateReply($"Age: {result.FaceAttributes.Age},Gender: {result.FaceAttributes.Gender},Smile: {result.FaceAttributes.Smile}");
                    await context.SendActivity(resultReply);  
                }
                else
                {
                    var resultReply = context.Request.CreateReply($"I can't see any person in here");
                    await context.SendActivity(resultReply);   
                }
            }

            var reply = context.Request.CreateReply("Please send me a picture and i'll identify the person \"Quit\" to stop");
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
