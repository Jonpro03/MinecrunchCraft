using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using minecrunch.models.Chunks;
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
                    Thread.Sleep(500);
                }
            });
            CreateWebHostBuilder(args).Build().Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
