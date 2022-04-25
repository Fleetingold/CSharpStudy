using FluentScheduler;
using FluentSchedulerDemo.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSchedulerDemo.Registries
{
    internal class SubErpCommissionBussinessAutoSyncRegistry : Registry
    {
        public SubErpCommissionBussinessAutoSyncRegistry()
        {
            Schedule<SubErpCommissionBussinessAutoSyncJob>().ToRunNow().AndEvery(3000).Milliseconds();
        }
    }
}
