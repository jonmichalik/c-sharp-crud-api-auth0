using Microsoft.EntityFrameworkCore;
using c_sharp_crud_api.Models;

namespace c_sharp_crud_api.Data
{
    public class TaskBoard : DbContext
    {
        public TaskBoard(DbContextOptions<TaskBoard> options)
            : base(options)
        { }

        public DbSet<Task> Tasks { get; set; }
    }
}
