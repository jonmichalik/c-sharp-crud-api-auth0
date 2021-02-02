using System;

namespace c_sharp_crud_api.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
    }

    public enum TaskStatus
    {
        Defined,
        InProgress,
        Complete
    }
}
