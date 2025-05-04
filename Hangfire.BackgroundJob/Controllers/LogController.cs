using Hangfire.BackgroundJob.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.BackgroundJob.Controllers
{
    //  A simple controller to trigger the background job
    [ApiController]
    [Route("[controller]")] //  Route will be  /Log
    public class LogController : ControllerBase
    {
        private readonly ILoggingService _loggingService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public LogController(ILoggingService loggingService, IBackgroundJobClient backgroundJobClient)
        {
            _loggingService = loggingService;
            _backgroundJobClient = backgroundJobClient;
        }

        //  Endpoint to trigger the logging job
        [HttpPost]
        public async Task<IActionResult> Log([FromBody] string message) // Take the log message from the body
        {
            // Enqueue the fire-and-forget job.  The *method* is called by Hangfire.
            _backgroundJobClient.Enqueue(() => _loggingService.LogMessageAsync(message));
            //await _loggingService.LogMessageAsync(message);
            //  Important:  Return *immediately*.  Don't wait for the job.
            return Ok($"Log message \"{message}\" enqueued.  Check /hangfire dashboard for status.");
        }
    }
}
