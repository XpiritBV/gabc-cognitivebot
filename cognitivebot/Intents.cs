using System;
using Microsoft.Bot.Builder.Core.Extensions;

namespace cognitivebot
{
    public static class Intents
    {
        public const string Train = "Train Images";

        public const string Suspects = "Suspects";
        public const string MurderWeapons = "Murder weapons";

        public const string IdentifySuspect = "Identify suspect";

        public const string IdentifyMurderWeapon = "Identify murder weapon";

        public const string MatchSuspect = "Match Suspect Description";

        public const string Quit = "Quit";

        public const string Yes = "Yes";
        public const string No = "No";
    }
}
