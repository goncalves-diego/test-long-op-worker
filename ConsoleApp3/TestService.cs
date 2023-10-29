using System.Diagnostics;

namespace ConsoleApp3
{
    public class TestService : ITestService
    {
        public async Task<Task> ExecuteAsync(string item, CancellationToken stoppingToken)
        {
            var watch = new Stopwatch();
            watch.Start();

            //download
            Task<Task> downloadTaskFactory = new Task<Task>(async () =>
            {
                Console.WriteLine($"inicio download {item} - {watch.ElapsedMilliseconds}");
                await Task.Delay(new Random().Next(1000, 3000), stoppingToken);
                Console.WriteLine($"fim download {item} - {watch.ElapsedMilliseconds}");
            });
            downloadTaskFactory.Start(TaskScheduler.Default);
            Task downloadTask = await downloadTaskFactory;
            await downloadTask;

            //upload
            Task<Task> uploadTaskFactory = new Task<Task>(async () =>
            {
                Console.WriteLine($"inicio upload {item} - {watch.ElapsedMilliseconds}");
                await Task.Delay(new Random().Next(2000, 10000), stoppingToken);
                Console.WriteLine($"fim upload {item} - {watch.ElapsedMilliseconds}");
            });
            uploadTaskFactory.Start(TaskScheduler.Default);
            Task uploadTask = await uploadTaskFactory;
            await uploadTask;

            //send tracking
            Task<Task> trackingTaskFactory = new Task<Task>(async () =>
            {
                Console.WriteLine($"inicio tracking {item} - {watch.ElapsedMilliseconds}");
                await Task.Delay(new Random().Next(200, 1000), stoppingToken);
                Console.WriteLine($"fim tracking {item} - {watch.ElapsedMilliseconds}");
            });
            trackingTaskFactory.Start(TaskScheduler.Default);
            Task trackingTask = await trackingTaskFactory;
            await trackingTask;

            //send email
            Task<Task> sendEmailTaskFactory = new Task<Task>(async () =>
            {
                Console.WriteLine($"inicio send {item} - {watch.ElapsedMilliseconds}");
                await Task.Delay(new Random().Next(300, 1000), stoppingToken);
                Console.WriteLine($"fim send {item} - {watch.ElapsedMilliseconds}");
            });
            sendEmailTaskFactory.Start(TaskScheduler.Default);
            Task sendTask = await sendEmailTaskFactory;
            await sendTask;

            return Task.CompletedTask;
        }
    }
}
