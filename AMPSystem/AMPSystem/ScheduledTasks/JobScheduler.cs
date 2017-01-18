using Quartz.Impl;

namespace AMPSchedules.ScheduledTasks
{
    public class JobScheduler
    {
        public static void Start()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
        }
    }
}