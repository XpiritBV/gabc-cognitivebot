using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Core.Extensions;
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
            var luisSection = Configuration.GetSection("LUIS");
            var botSection = Configuration.GetSection("Bot");

            var botStateStorage = new MemoryStorage();
            var stateSettings = new StateSettings
            {
                LastWriterWins = true,
                WriteBeforeSend = true
            };

            var adapter = new BotFrameworkAdapter(new SimpleCredentialProvider
            {
                AppId = botSection.GetValue<string>("AppId"),
                Password = botSection.GetValue<string>("AppPwd")
            })
                .Use(new ConversationState<DetectiveBotContext>(botStateStorage, stateSettings))
                .Use(new UserState<UserStateModel>(botStateStorage, stateSettings));


            services.AddSingleton(adapter);
            services.AddTransient<IBot, DetectiveBot>();
            services.AddSingleton(new HttpClient());
            services.AddMvc();
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
