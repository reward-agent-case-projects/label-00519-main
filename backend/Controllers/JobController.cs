using Backend.Api.Models;
using Backend.Api.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public JobController(
            ILogger<JobController> logger,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager)
        {
            _logger = logger;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        /// <summary>
        /// Trigger a fire-and-forget job
        /// </summary>
        [HttpPost("fire-and-forget")]
        public IActionResult FireAndForget([FromBody] JobRequest request)
        {
            var jobId = _backgroundJobClient.Enqueue<ITaskService>(x => x.ExecuteTask(request.TaskType, request.Payload));
            _logger.LogInformation("Enqueued Fire-and-Forget Job: {JobId}", jobId);
            return Ok(new { JobId = jobId, Message = "Job enqueued successfully" });
        }

        /// <summary>
        /// Schedule a delayed job
        /// </summary>
        [HttpPost("delayed")]
        public IActionResult Delayed([FromBody] DelayedJobRequest request)
        {
            var jobId = _backgroundJobClient.Schedule<ITaskService>(
                x => x.ExecuteTask(request.TaskType, request.Payload),
                TimeSpan.FromSeconds(request.DelaySeconds));
            
            _logger.LogInformation("Scheduled Delayed Job: {JobId} to run in {DelaySeconds}s", jobId, request.DelaySeconds);
            return Ok(new { JobId = jobId, Message = "Job scheduled successfully" });
        }

        /// <summary>
        /// Create or update a recurring job
        /// </summary>
        [HttpPost("recurring")]
        public IActionResult Recurring([FromBody] RecurringJobRequest request)
        {
            _recurringJobManager.AddOrUpdate<ITaskService>(
                request.JobId,
                x => x.ExecuteTask(request.TaskType, request.Payload),
                request.CronExpression);

            _logger.LogInformation("Recurring Job {JobId} configured with Cron: {Cron}", request.JobId, request.CronExpression);
            return Ok(new { JobId = request.JobId, Message = "Recurring job configured successfully" });
        }

        /// <summary>
        /// Delete a recurring job
        /// </summary>
        [HttpDelete("recurring/{jobId}")]
        public IActionResult DeleteRecurring(string jobId)
        {
            _recurringJobManager.RemoveIfExists(jobId);
            _logger.LogInformation("Recurring Job {JobId} removed", jobId);
            return Ok(new { JobId = jobId, Message = "Recurring job removed successfully" });
        }
    }
}
