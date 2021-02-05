using Microsoft.EntityFrameworkCore;
using System.Linq;
using c_sharp_crud_api.Models;

namespace c_sharp_crud_api.Data
{
    public class TaskBoard : DbContext
    {
        public TaskBoard(DbContextOptions<TaskBoard> options)
            : base(options)
        { }

        public DbSet<Task> Tasks { get; set; }

        public int NextId() => Tasks.Any() ? Tasks.Max(t => t.Id) + 1 : 1;
    }
}
