using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Performance
{
    public static class Performance
    {
        public static async Task<double> GetCpuUsageForProcess()
        {
            
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

            await Task.Delay(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;

            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            
            return cpuUsageTotal * 100;
        }
    }
}
