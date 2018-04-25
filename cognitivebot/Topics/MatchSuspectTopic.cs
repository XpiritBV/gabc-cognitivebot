using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cognitivebot.Services;
using Microsoft.ProjectOxford.Face;
using System.Linq;
using Microsoft.ProjectOxford.Face.Contract;

namespace cognitivebot.Topics
{
    public class MatchSuspectTopic : ITopic
    {
        IFaceRecognitionService faceRecognitionService;

        public MatchSuspectTopic(IFaceRecognitionService faceRecognitionService)
        {
            this.faceRecognitionService = faceRecognitionService;
        }

        public enum TopicState
        {
            started,
            askedAge,
            askedSex,
            askedHairColor,
            askedFacialHair,
            checkSuspectPicture,
            unknown
        }

        public TopicState State
        {
            get;
            set;
        } = TopicState.unknown;

        public string Age { get; set; }
        public string Sex { get; set; }
        public string HairColor { get; set; }
        public bool FacialHair { get; set; }


        public string Name { get => "MatchSuspectTopic"; }

        public async Task<bool> ContinueTopic(DetectiveBotContext context)
        {
            switch(State)
            {
                case TopicState.started:
                    return await AskAgeAsync(context);
                case TopicState.askedAge:
                    return await AskSex(context);
                case TopicState.askedSex:
                    return await AskHairColor(context);
                case TopicState.askedHairColor:
                    return await AskFacialHair(context);
                case TopicState.askedFacialHair:
                    return await SubjectSummary(context);
                case TopicState.checkSuspectPicture:
                    return await CheckSuspectPicture(context);
                case TopicState.unknown:
                default:
                    return await StartTopic(context);
            } 
        }

        private async Task<bool> AskAgeAsync(DetectiveBotContext context)
        {
            var reply = BotReplies.ReplyWithOptions("What is the age of the suspect?", new List<string>() { "<20", "20-30", "31-40", "41-50", ">50" }, context);
            await context.SendActivity(reply);
            State = TopicState.askedAge;

            return true;
        }

        private async Task<bool> AskSex(DetectiveBotContext context)
        {
            Age = context.Request.Text;

            var reply = BotReplies.ReplyWithOptions("What is the sex of the suspect?", new List<string>() { "male", "female" }, context);
            await context.SendActivity(reply);
            State = TopicState.askedSex;

            return true;
        }

        private async Task<bool> AskHairColor(DetectiveBotContext context)
        {
            Sex = context.Request.Text;

            var reply = BotReplies.ReplyWithOptions("What is the hair color of the suspect", new List<string>() { "Blond", "Brown", "Black", "Red", "Gray", "Bald" }, context);
            await context.SendActivity(reply);
            State = TopicState.askedHairColor;

            return true;
        }

        private async Task<bool> AskFacialHair(DetectiveBotContext context)
        {
            HairColor = context.Request.Text;

            var reply = BotReplies.ReplyWithOptions("Does the suspect have facial hair", new List<string>() { Intents.Yes, Intents.No }, context);
            await context.SendActivity(reply);
            State = TopicState.askedFacialHair;

            return true;
        }

        private async Task<bool> SubjectSummary(DetectiveBotContext context)
        {
            bool result = false;
            bool.TryParse(context.Request.Text, out result);

            FacialHair = result;

            await context.SendActivity(context.Request.CreateReply($"Ok Suspect description is:"));
            await context.SendActivity(context.Request.CreateReply($"Age: {Age}"));
            await context.SendActivity(context.Request.CreateReply($"Sex {Sex}"));
            await context.SendActivity(context.Request.CreateReply($"Hair color: {HairColor}"));
            await context.SendActivity(context.Request.CreateReply($"Facial Hair {FacialHair.ToString()}"));

            State = TopicState.checkSuspectPicture;

            return await ContinueTopic(context);
        }

        private async Task<bool> CheckSuspectPicture(DetectiveBotContext context)
        {
            if(context.Request.Attachments?.Count > 0)
            {
                try
                {
                    var match = 0;
                    var face = await faceRecognitionService.GetFaceAttributes(context.Request.Attachments[0].ContentUrl);
                    if(face != null)
                    {
                        //check sex
                        if(face.FaceAttributes.Gender.ToLower() == Sex.ToLower())
                        {
                            match += 25;
                        }

                        //check age
                        switch ((int)Math.Round(face.FaceAttributes.Age))
                        {
                            case int n when (n <= 20):
                                if (Age == "<20") { match += 25; }
                                break;
                            case int n when (n > 20 && n <= 30):
                                if (Age == "20-30") { match += 25; }
                                break;
                            case int n when (n > 30 && n <= 40):
                                if (Age == "31-40") { match += 25; }
                                break;
                            case int n when (n > 40 && n <= 50):
                                if (Age == "41-50") { match += 25; }
                                break;
                            case int n when (n > 50):
                                if(Age == ">50") { match += 25; }
                                break;
                        }

                        //check hair color
                        if (HairColor.ToLower() == "bald" && (face.FaceAttributes.Hair.Bald > 0.7))
                        { 
                            match += 25; 
                        }
                        else
                        {
                            if(face.FaceAttributes.Hair.HairColor != null &&
                               face.FaceAttributes.Hair.HairColor.Any(color => color.Color == (HairColorType)Enum.Parse(typeof(HairColorType),HairColor) 
                                                                      && color.Confidence > 0.7))
                            {
                                match += 25;
                            }
                        }

                        //check facial hair
                        if(FacialHair && (face.FaceAttributes.FacialHair.Beard > 0.7 || 
                                          face.FaceAttributes.FacialHair.Moustache > 0.7 ||
                                          face.FaceAttributes.FacialHair.Sideburns > 0.7))
                        {
                            match += 25;
                        }
                        else
                        {
                            if (!FacialHair && (face.FaceAttributes.FacialHair.Beard < 0.5 &&
                                               face.FaceAttributes.FacialHair.Moustache < 0.5 &&
                                               face.FaceAttributes.FacialHair.Sideburns < 0.5))
                            {
                                match += 25;
                            }
                            
                        }
                        
                    }

                    await context.SendActivity(context.Request.CreateReply($"This person matches the description for {match.ToString()}%"));


                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                    await context.SendActivity(context.Request.CreateReply($"Error processing this image. Are you sure it contained a face?"));
                }

            }
            await context.SendActivity(context.Request.CreateReply($"Take a picture of a suspect and i'll check how far it matches the description"));

            State = TopicState.checkSuspectPicture;

            return true;
        }

        public async Task<bool> ResumeTopic(DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply("Welcome back. We'll have to start over matching suspects.");
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
