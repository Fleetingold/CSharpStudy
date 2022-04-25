using FluentScheduler;

namespace FluentSchedulerDemo.Jobs
{
    internal class SubErpCommissionBussinessAutoSyncJob : IJob
    {
        public void Execute()
        {
            Console.WriteLine("Start SubErpCommissionBussinessAutoSyncJob!");

            Console.WriteLine("SubErpCommissionBussinessAutoSyncJob Success!");
        }
    }
}
