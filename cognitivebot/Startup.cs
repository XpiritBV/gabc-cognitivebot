using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.LUIS;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Cognitive.LUIS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace cognitivebot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            services.AddBot<DetectiveBot>(options =>
            {
                options.CredentialProvider = new SimpleCredentialProvider(Configuration.GetSection(MicrosoftAppCredentials.MicrosoftAppIdKey)?.Value, Configuration.GetSection(MicrosoftAppCredentials.MicrosoftAppPasswordKey)?.Value);
                var middleware = options.Middleware;

                middleware.Add(new UserState<UserData>(new MemoryStorage()));
                middleware.Add(new ConversationState<ConversationData>(new MemoryStorage()));
                middleware.Add(new RegExpRecognizerMiddleware()
                                .AddIntent("showAlarms", new Regex("show alarms(.*)", RegexOptions.IgnoreCase))
                                .AddIntent("addAlarm", new Regex("add alarm(.*)", RegexOptions.IgnoreCase))
                                .AddIntent("deleteAlarm", new Regex("delete alarm(.*)", RegexOptions.IgnoreCase))
                                .AddIntent("help", new Regex("help(.*)", RegexOptions.IgnoreCase))
                                .AddIntent("cancel", new Regex("cancel(.*)", RegexOptions.IgnoreCase))
                                .AddIntent("confirmYes", new Regex("(yes|yep|yessir|^y$)", RegexOptions.IgnoreCase))
                                .AddIntent("confirmNo", new Regex("(no|nope|^n$)", RegexOptions.IgnoreCase)));

            });

            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseBotFramework();
        }
    }
}
