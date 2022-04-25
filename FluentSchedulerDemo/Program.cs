// See https://aka.ms/new-console-template for more information
using FluentSchedulerDemo.Registries;

Console.WriteLine("Hello, World!");

//1、Plan A
//FluentScheduler.JobManager.Initialize();

//FluentScheduler.JobManager.AddJob(
//    () => Console.WriteLine("1 minutes just passed."),
//    s => s.ToRunEvery(1).Minutes()
//);

//2、Plan B
FluentScheduler.Registry registry = new SubErpCommissionBussinessAutoSyncRegistry();

FluentScheduler.JobManager.Initialize(registry);

Console.ReadKey();