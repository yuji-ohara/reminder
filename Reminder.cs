using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;
using static Quartz.MisfireInstruction;

namespace Reminder
{
    public class Reminder
    {
        StdSchedulerFactory schedulerFactory { get; set; }
        IScheduler scheduler { get; set; }

        private static Action _action;

        private static Reminder _instance;
        public static Reminder Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Reminder();

                return _instance;
            }
        }

        public static async Task Init()
        {
            Instance.schedulerFactory = new StdSchedulerFactory();
            Instance.scheduler = await Instance.schedulerFactory.GetScheduler();
        }

        public async Task AddDrinkWaterJob(Action action)
        {
            _action = action;
            IJobDetail jobDetails = JobBuilder.Create<DrinkWaterTask>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithCronSchedule("0 0 0/1 1/1 * ? *")
                .Build();
            await scheduler.ScheduleJob(jobDetails, trigger);
            await scheduler.Start();
        }

        public async Task AddQuickDrinkWaterJob(Action action)
        {
            _action = action;
            IJobDetail jobDetails = JobBuilder.Create<DrinkWaterTask>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(DateTime.Now.AddMinutes(5))
                .Build();
            await scheduler.ScheduleJob(jobDetails, trigger);
            await scheduler.Start();
        }

        public static Task Execute()
        {
            return Task.Run(_action);
        }
    }
}
