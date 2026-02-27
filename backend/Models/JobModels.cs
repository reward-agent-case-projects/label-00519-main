using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Models
{
    public class JobRequest
    {
        [Required]
        public string TaskType { get; set; } = string.Empty;
        
        [Required]
        public string Payload { get; set; } = string.Empty;
    }

    public class DelayedJobRequest : JobRequest
    {
        [Range(1, int.MaxValue)]
        public int DelaySeconds { get; set; }
    }

    public class RecurringJobRequest : JobRequest
    {
        [Required]
        public string JobId { get; set; } = string.Empty;

        [Required]
        public string CronExpression { get; set; } = string.Empty;
    }
}
