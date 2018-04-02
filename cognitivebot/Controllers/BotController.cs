using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace cognitivebot.Controllers
{
    [Route("api/[controller]")]
    public class BotController : Controller
    {
        private readonly BotFrameworkAdapter _adapter;
        private readonly IBot _bot;
        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="bot"></param>
        /// <param name="loggerFactory"></param>
        public BotController(BotFrameworkAdapter adapter, IBot bot, TelemetryClient telemetryClient, ILoggerFactory loggerFactory)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _logger = loggerFactory?.CreateLogger(GetType().Name);
        }

        /// <summary>
        /// Takes the incoming activity and delegates it to <see cref="_bot"/>.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Activity activity)
        {
            var telemetry = new DependencyTelemetry("Bot activity", "Bot", "Bot call", activity.Action);
            try
            {
                await _adapter.ProcessActivity(Request.Headers["Authorization"].FirstOrDefault(), activity,
                    _bot.OnReceiveActivity);
                telemetry.Success = true;
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                telemetry.Success = false;
                _logger?.LogWarning(ex, "Unauthorized access attempt.");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                telemetry.Success = false;
                _logger?.LogError(ex, "Unknown error.");
                return StatusCode(500);
            }
            finally
            {
                _telemetryClient.TrackDependency(telemetry);
            }
        }
    }
}
