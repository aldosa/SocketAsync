
using System.ComponentModel;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Threading;

namespace SocketAsync
{
    class Program
    {
        static BackgroundWorker bwServer = new BackgroundWorker();
        static BackgroundWorker bwClient = new BackgroundWorker();

        static void Main(string[] args)
        {
            bwServer.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args1)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    Process serverProcess = new Process();
                    serverProcess.StartInfo.FileName = "SocketAsyncServer.exe";
                    serverProcess.StartInfo.Arguments = "";
                    serverProcess.Start();
                    serverProcess.WaitForExit();// Waits here for the process to exit.

                });

            bwClient.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args1)
                {
                    Process clientProcess = new Process();
                    clientProcess.StartInfo.FileName = "SocketAsyncClient.exe";
                    clientProcess.StartInfo.Arguments = "127.0.0.1 9900 20";
                    clientProcess.Start();
                    clientProcess.WaitForExit();// Waits here for the process to exit.

                });

            bwServer.RunWorkerAsync();

            Thread.Sleep(2000);


            bwClient.RunWorkerAsync();

            Thread.Sleep(40000);


        }
    }
}
