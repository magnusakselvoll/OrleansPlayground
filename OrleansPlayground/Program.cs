using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace OrleansPlayground
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // First, configure and start a local silo
            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            var silo = new SiloHost("TestSilo", siloConfig);
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            Console.WriteLine("Silo started.");

            // Then configure and connect a client.
            var clientConfig = ClientConfiguration.LocalhostSilo();
            var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
            client.Connect().Wait();

            Console.WriteLine("Client connected.");

            int numberOfGrains = 200;

            var tasks = new Task[numberOfGrains];
            for (int i = 0; i < numberOfGrains; i++)
            {
                var friend = client.GetGrain<IGrain1>(i);
                Task<string> task = friend.SayHello();
                var taskId = i;
                task.ContinueWith(helloTask => { Console.WriteLine($"{taskId}: {helloTask.Result}"); });
                tasks[i] = task;
            }

            Task.WaitAll(tasks);
            
            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();

            // Shut down
            client.Close();
            silo.ShutdownOrleansSilo();
        }
    }
}
