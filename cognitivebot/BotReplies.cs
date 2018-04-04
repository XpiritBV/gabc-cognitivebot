using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;

namespace cognitivebot
{
    public static class BotReplies
    {
        public static Activity ReplyWithOptions(string question, List<string> options, DetectiveBotContext context)
        {
            var reply = context.Request.CreateReply(question);

            reply.SuggestedActions = new SuggestedActions(
                actions: options.Select(option =>
                         new CardAction(type: ActionTypes.ImBack,
                                        title: option,
                                        value: option)).ToList());
            
            return reply;
            
        }

    }
}
