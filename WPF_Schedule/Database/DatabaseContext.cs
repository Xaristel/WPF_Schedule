using System.Data.Entity;
using WPF_Schedule.Model;

namespace WPF_Schedule.Database
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection")
        {

        }

        public virtual DbSet<Schedule> Schedules { get; set; }
    }
}
