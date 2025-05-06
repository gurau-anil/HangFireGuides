using Hangfire.BackgroundJob.Services;
using HangFire.Configuration.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.BackgroundJob.Controllers
{
    //  A simple controller to trigger the background job
    [ApiController]
    [Route("[controller]")] //  Route will be  /Log
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
        public async Task<IActionResult> Log([FromBody] string message) // Take the log message from the body
        {
            _backgroundService.Enqueue(() => _loggingService.LogMessageAsync(message));

            //  Important:  Return *immediately*.  Don't wait for the job.
            return Ok($"Log message \"{message}\" enqueued.  Check /hangfire dashboard for status.");
        }


    }
}
