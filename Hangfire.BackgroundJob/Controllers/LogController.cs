using Hangfire.Services.Services;
using HangFire.Configuration.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Services.Controllers
{
    //  A simple controller to trigger the background job
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILoggingService _loggingService;
        private readonly IHangfireBackgroundService _backgroundService;

        public LogController(ILoggingService loggingService, IHangfireBackgroundService backgroundService)
        {
            _loggingService = loggingService;
            _backgroundService = backgroundService;
        }

        //  Endpoint to trigger the logging job
        [HttpPost]
        [Route("log")]
        public async Task<IActionResult> Log([FromBody] string message) // Take the log message from the body
        {
            _backgroundService.Enqueue(() => _loggingService.LogMessageAsync(message));

            //// run a job after certain time
            //_backgroundService.Schedule(() => _loggingService.LogMessageAsync(message), TimeSpan.FromMinutes(10));

            //// run the Every day at 12:00 AM
            //_backgroundService.AddRecurring("jobId",() => _loggingService.LogMessageAsync(message), "0 0 * * *");

            //  Important:  Return *immediately*.  Don't wait for the job.
            return Ok($"Log message \"{message}\" enqueued.  Check /hangfire dashboard for status.");
        }

    }
}
