using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static List<Chunk> ChunkCache = new List<Chunk>();
        private static Timer trTimer, queueTimer;

        public static void Main(string[] args)
        {
            TaskRunner tr = TaskRunner.Instance;
            trTimer = new Timer()
            {
                Enabled = true,
                Interval = 300
            };
            trTimer.Elapsed += new ElapsedEventHandler(tr.RunTasks);

            queueTimer = new Timer()
            {
                Enabled = true,
                Interval = 100
            };
            queueTimer.Elapsed += new ElapsedEventHandler(tr.UpdateQueues);

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
