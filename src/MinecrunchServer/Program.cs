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
            TerrainWorker terrainWorker = new TerrainWorker(5);
            FaceCalcWorker faceCalcWorker = new FaceCalcWorker(2);
            ChunkSaveWorker chunkSaveWorker = new ChunkSaveWorker();

            Task.Run(() => {
                while (true)
                {
                    terrainWorker.Run();
                    faceCalcWorker.Run();
                    chunkSaveWorker.Run();
                    Thread.Sleep(100);
                }
            });

            CreateWebHostBuilder(args).Build().Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
