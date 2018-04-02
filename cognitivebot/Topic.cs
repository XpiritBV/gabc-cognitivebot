using System;
namespace cognitivebot
{
    public class Topic
    {
        public string ActiveTopic { get; set; } = Activities.None;
        public string ActiveUserId { get; set; }
    }

    public class UserStateModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public static class Activities
    {
        public const string None = nameof(None);

        public const string TrainSuspects = nameof(TrainSuspects);

        public const string CheckSuspects = nameof(CheckSuspects);

        public const string GetFaceAttributes = nameof(GetFaceAttributes);
    }
}
