using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceStack.Redis;
using ServiceStack;
using ServiceStack.Text;
using ServiceStack.DataAnnotations;
using SimpleInjector;

namespace RedisSentinelAlpha
{
    static class Program
    {
        private static IRedisClientsManager redisManager;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Bootstrap();
            Application.Run(new Form1(GetRedisClientsManager()));
        }

       

        private static IRedisClientsManager GetRedisClientsManager()
        {


            //var sentinelHosts = new[] { "somredis01.zerochaos.local:26379","somredis02.zerochaos.local:26379","somredis03.zerochaos.local:26379" };
            var sentinelHosts = new[] { "supdevredis01.zcdev.local:26379", "supdevredis01.zcdev.local:26380", "supdevredis01.zcdev.local:26381" };
            //var sentinelHosts = new[] { "supredis01.zcdev.local:26379", "supredis01.zcdev.local:26380", "supredis01.zcdev.local:26381" };
            var sentinel = new RedisSentinel(sentinelHosts, masterName: "zcredismaster");

            if (redisManager != null)
                return redisManager;
            else
            {
                // By default RedisSentinel uses a PooledRedisClientManager
                return sentinel.Start();
            }
        }
    }
}
