using Quartz;
using System;
using System.Threading.Tasks;

namespace Reminder
{
    public class DrinkWaterTask : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(Reminder.Execute);
        }
    }
}
