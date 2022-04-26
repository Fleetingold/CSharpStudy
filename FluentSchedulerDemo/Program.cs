// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//1、Plan A
//FluentScheduler.JobManager.Initialize();

//FluentScheduler.JobManager.AddJob(
//    () => Console.WriteLine("1 minutes just passed."),
//    s => s.ToRunEvery(1).Minutes()
//);

//2、Plan B
//FluentScheduler.Registry registry = new FluentSchedulerDemo.Registries.SubErpCommissionBussinessAutoSyncRegistry();

//FluentScheduler.JobManager.Initialize(registry);

//3、Plan C
var registry = new FluentScheduler.Registry();
registry.Schedule<FluentSchedulerDemo.Jobs.SubErpCommissionBussinessAutoSyncJob>().ToRunNow().AndEvery(3000).Milliseconds();

FluentScheduler.JobManager.Initialize(registry);

Console.ReadKey();