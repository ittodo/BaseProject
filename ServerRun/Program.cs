using Memory;
using System;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Socket;
using Socket.Serialize;

namespace ServerRun
{
    class Program
    {

        static EventWaitHandle handle = new EventWaitHandle(false , EventResetMode.AutoReset);
        static void Main(string[] args)
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {

            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                // Create Deamon
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {

            }


            Run.Execute();

            Task.Run(async() =>
            {
                do
                {
                    await Task.Delay(5000);
                    var value = await Performance.Performance.GetCpuUsageForProcess();
                    Console.WriteLine(value + " % Cpu Usage");
                } while (true);
            });

            handle.WaitOne();
            Socket.Run.CleanUp();
            System.GC.Collect(5, GCCollectionMode.Forced, true);
            System.Threading.Thread.Sleep(10000);
        }

        static public void CleanUp()
        {
            handle.Set();
        }

    }
}
