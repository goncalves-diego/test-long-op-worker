namespace ConsoleApp3
{
    public interface ITestService
    {
        Task<Task> ExecuteAsync(string item, CancellationToken stoppingToken);
    }
}
