using LiveClientForBlender;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LiveControlHub
{
    public class Program
    {
        public static ObjectController ObjectController = new ObjectController();

        public static void Main(string[] args)
        {
            //Thread t = new Thread(new ThreadStart(ThreadProc));
            //t.Start();
            ObjectController.Start();
            CreateHostBuilder(args).Build().Run();
        }

        private static void ThreadProc()
        {
            ObjectController.Start();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });



    }
}
