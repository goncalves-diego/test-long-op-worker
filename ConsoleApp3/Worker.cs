using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace ConsoleApp3
{
    public class Worker : BackgroundService
    {
        private ITestService _testService;
        public Worker(ITestService testService)
        {
            _testService = testService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var numberOfHandlers = 1;
            Console.WriteLine("Qual solução de paralelismo você quer testar? (1, 2, 3 ou 4)");
            var value = Console.ReadLine();
            var solution = Convert.ToInt32(value);

            Console.WriteLine("Quantas mensagens você quer gerar? (1 a n)");
            value = Console.ReadLine();
            var numberOfMessages = Convert.ToInt32(value);

            if (solution == 4)
            {
                Console.WriteLine("Quantas threads você quer usar para processar as mensagens? (1 a n)");
                value = Console.ReadLine();
                numberOfHandlers = Convert.ToInt32(value);
            }

            var lst = new List<string>();
            for (int i = 0; i < numberOfMessages; i++)
                lst.Add($"Item {i}");

            while (!stoppingToken.IsCancellationRequested)
            {
                var watch = new Stopwatch();
                watch.Start();

                Console.WriteLine($"Iniciando solução {solution}");
                Console.WriteLine("---------------INÍCIO-----------------");

                if (solution == 1)
                    Parallel.ForEach(lst, async (item, cancellationToken) =>
                    {
                        await _testService.ExecuteAsync(item, stoppingToken);
                    });

                if (solution == 2)
                    await Parallel.ForEachAsync(lst, async (item, cancellationToken) =>
                    {
                        await _testService.ExecuteAsync(item, stoppingToken);
                    });

                if (solution == 3)
                {
                    var tasks = lst.Select(async (item) =>
                    {
                        var x = await _testService.ExecuteAsync(item, stoppingToken);
                        return x;
                    });
                    var results = await Task.WhenAll(tasks);
                }

                if (solution == 4)
                {
                    List<Task<Task>> allHandlersTasks = new List<Task<Task>>();
                    for (int i = 0; i < numberOfHandlers; i++)
                    {
                        var tasks = lst.Select(async (item) =>
                        {
                            var x = await _testService.ExecuteAsync(item, stoppingToken);
                            return x;
                        });

                        allHandlersTasks.AddRange(tasks);
                    }
                    var results = await Task.WhenAll(allHandlersTasks);
                }

                watch.Stop();

                var tps = ((numberOfMessages * numberOfHandlers) / (watch.ElapsedMilliseconds / 1000));
                Console.WriteLine($"---------------------------TPS: {tps}");
                Console.WriteLine("---------------FIM--------------------");
                Thread.Sleep(1000);
            }
        }
    }
}
