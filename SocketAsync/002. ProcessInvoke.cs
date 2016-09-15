using System.Diagnostics;

namespace SocketAsync
{
    public class ProcessInvoke
    {
        public static void Test()
        {
            Process serverProcess = new Process();
            // Configure the process using the StartInfo properties.
            serverProcess.StartInfo.FileName = "SocketAsyncServer.exe";
            serverProcess.StartInfo.Arguments = "";
            //serverProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            serverProcess.Start();
            serverProcess.WaitForExit();// Waits here for the process to exit.



        }


    }
}
