using System.Threading;
using System.ComponentModel;

namespace SocketAsync
{
    /// <summary>
    /// borrowed from http://stackoverflow.com/questions/363377/how-do-i-run-a-simple-bit-of-code-in-a-new-thread
    /// </summary>
    class BackgroundWorkerTest
    {
        BackgroundWorker bw = new BackgroundWorker();

        public string Output;

        public void Test()
        {
            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(
                delegate (object o, ProgressChangedEventArgs args1)
                {
                    Output = (string.Format("{0}% Completed", args1.ProgressPercentage));
                });

            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate (object o, RunWorkerCompletedEventArgs args1)
            {
                Output = ("Finished!");
            });


            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args1)
            {
                BackgroundWorker b = o as BackgroundWorker;

                // do some simple processing for 10 seconds
                for (int i = 1; i <= 10; i++)
                {
                    // report the progress in percent
                    b.ReportProgress(i * 10);
                    Thread.Sleep(1000);
                }

            });


            bw.RunWorkerAsync();

        }

    }

}
