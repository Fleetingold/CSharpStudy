// See https://aka.ms/new-console-template for more information
using Serilog;

Console.WriteLine("Hello, World!");

//0、Serilog日志配置
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console(new Serilog.Formatting.Compact.RenderedCompactJsonFormatter())
    .CreateLogger();

FluentScheduler.JobManager.JobStart += info => Log.Information($"{info.Name}: started");
FluentScheduler.JobManager.JobEnd += info => Log.Information($"{info.Name}: ended ({info.Duration})");
FluentScheduler.JobManager.JobException += info => Log.Error("An error just happened with a scheduled job: " + info.Exception);

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