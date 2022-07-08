using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("FireAndForget")]
        public IActionResult FireAndForget()
        {
            var jobFireForget = BackgroundJob.Enqueue(() => 
                Console.WriteLine($"EXECUTE: One time - {DateTime.Now}")
                );
            return Ok(jobFireForget);
        }

        [HttpGet("Delayed")]
        public IActionResult Delayed()
        {
            BackgroundJob.Schedule(methodCall: () =>
                Console.WriteLine($"EXECUTE: After 10 seconds - {DateTime.Now}")
                ,
                TimeSpan.FromSeconds(10)
            );
            return Ok();
        }

        [HttpGet("Recurring")]
        public IActionResult Recurring()
        {
            RecurringJob.AddOrUpdate("Log Sync",
                () => Console.WriteLine($"EXECUTE: Recurring every 1 minute - {DateTime.Now}"),
                Cron.Minutely
            );
            return Ok();
        }
    }
}