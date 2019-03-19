using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using MinecrunchServer.Logic;

namespace MinecrunchServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TaskRunner trInstance = TaskRunner.Instance;
            Task.Run(() => {
                while (true)
                {
                    trInstance.UpdateQueues();
                    Thread.Sleep(500); // Reduce to run the server harder
                }
            });
            CreateWebHostBuilder(args).Build().Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
