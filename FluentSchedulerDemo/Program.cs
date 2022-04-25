// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

FluentScheduler.JobManager.Initialize();

FluentScheduler.JobManager.AddJob(
    () => Console.WriteLine("1 minutes just passed."),
    s => s.ToRunEvery(1).Minutes()
);

Console.ReadKey();