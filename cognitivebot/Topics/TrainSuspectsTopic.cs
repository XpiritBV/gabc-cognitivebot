using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cognitivebot.Services;

namespace cognitivebot.Topics
{
    public class TrainSuspectsTopic : ITopic
    {
        public enum TopicState
        {
            unknown,
            started,
            askedForPicture,
            checkEsistingPerson,
            askedForName,
            finished
        }

        public TopicState State{ get; set; } = TopicState.unknown;
        public string SuspectName { get; set; }
        public Guid SuspectId { get; set; }


        public string Name { get => "TrainTopic"; }

        public async Task<bool> ContinueTopic(DetectiveBotContext context)
        {
            switch(State)
            {
                case TopicState.started:
                    return await AskForPicture(context);
                case TopicState.askedForPicture:
                    return await CheckPictureAndAskForName(context);
                case TopicState.checkEsistingPerson:
                    return await CheckExistingPerson(context);
                case TopicState.askedForName:
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
            var reply = context.Request.CreateReply("Please send me a picture of a suspect");
            await context.SendActivity(reply);
            State = TopicState.askedForPicture;

            return true;
        }

        private async Task<bool> CheckPictureAndAskForName(DetectiveBotContext context)
        {
            if(context.Request.Attachments != null && context.Request.Attachments.Count > 0)
            {
                string photo = context.Request.Attachments[0].ContentUrl;

                FaceRecognitionService faceRecognitionService = new FaceRecognitionService();
                var person = await faceRecognitionService.IdentifyPerson(photo);

                if(!string.IsNullOrEmpty(person))
                {
                    SuspectName = person;
                    var reply2 = BotReplies.ReplyWithOptions($"Is this {person}?", new List<string>() { Intents.Yes, Intents.No }, context);
                    await context.SendActivity(reply2);
                    State = TopicState.checkEsistingPerson;
                    return true;  
                }
                else
                {
                    var reply = context.Request.CreateReply("What is the name of this suspect?");
                    await context.SendActivity(reply);
                    State = TopicState.askedForName;
                    return true;

                }
            }

        }

        private Task<bool> CheckExistingPerson(DetectiveBotContext context)
        {
            if (context.RecognizedIntents.TopIntent?.Name == Intents.Yes)
            {
                FaceRecognitionService faceRecognitionService = new FaceRecognitionService();
                faceRecognitionService.AddPhotoToExistingPerson()
                
            }
            
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
            var reply = context.Request.CreateReply("Welcome back. let's continue adding some suspects");
            await context.SendActivity(reply);
            State = TopicState.started;

            return false;
        }

        public async Task<bool> StartTopic(DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply("Let's learn something about the suspects you know.");
            await context.SendActivity(reply);
            State = TopicState.started;

            return await this.ContinueTopic(context);
        }
    }
}
