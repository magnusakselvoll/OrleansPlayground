using System;
using System.Threading;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace GrainsCollection
{
    /// <summary>
    /// Grain implementation class Grain1.
    /// </summary>
    public class Grain1 : Grain, IGrain1
    {
        public static readonly Random Rand = new Random();

        public Task<string> SayHello()
        {
            var sleepTime = Rand.Next(150);
            Thread.Sleep(sleepTime);
            return Task.FromResult($"Hello World! (slept {sleepTime}ms)");
        }
    }
}
