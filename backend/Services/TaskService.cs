namespace Backend.Api.Services
{
    public interface ITaskService
    {
        void ExecuteTask(string taskType, string payload);
    }

    public class TaskService : ITaskService
    {
        private readonly ILogger<TaskService> _logger;

        public TaskService(ILogger<TaskService> logger)
        {
            _logger = logger;
        }

        public void ExecuteTask(string taskType, string payload)
        {
            _logger.LogInformation("Executing task {TaskType} with payload: {Payload}", taskType, payload);
            
            // Simulate work
            switch (taskType.ToLower())
            {
                case "email":
                    SendEmail(payload);
                    break;
                case "data-process":
                    ProcessData(payload);
                    break;
                default:
                    _logger.LogWarning("Unknown task type: {TaskType}", taskType);
                    break;
            }
        }

        private void SendEmail(string payload)
        {
            // Simulate email sending
            _logger.LogInformation("Sending email to {Payload}...", payload);
            Thread.Sleep(1000);
            _logger.LogInformation("Email sent successfully.");
        }

        private void ProcessData(string payload)
        {
            // Simulate data processing
            _logger.LogInformation("Processing data: {Payload}...", payload);
            Thread.Sleep(2000);
            _logger.LogInformation("Data processed successfully.");
        }
    }
}
