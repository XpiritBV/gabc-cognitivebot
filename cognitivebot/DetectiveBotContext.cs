using System;
using cognitivebot.Topics;

namespace cognitivebot
{
    public class DetectiveBotContext
    {
        public ITopic ActiveTopic { get; set; }
        public string ActiveUserId { get; set; }
    }

    public class UserStateModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public static class Activities
    {
        public const string Default = nameof(Default);

        public const string TrainSuspects = nameof(TrainSuspects);

        public const string CheckSuspects = nameof(CheckSuspects);

        public const string GetFaceAttributes = nameof(GetFaceAttributes);
    }
}
