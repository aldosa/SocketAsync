using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Globalization;

namespace SocketAsyncServer
{
    delegate void ProcessData(SocketAsyncEventArgs args);

    /// <summary>
    /// Token for use with SocketAsyncEventArgs.
    /// </summary>
    internal sealed class Token : IDisposable
    {
        private Socket connection;

        private StringBuilder sb;

        private Int32 currentIndex;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="connection">Socket to accept incoming data.</param>
        /// <param name="bufferSize">Buffer size for accepted data.</param>
        internal Token(Socket connection, Int32 bufferSize)
        {
            this.connection = connection;
            this.sb = new StringBuilder(bufferSize);
        }

        /// <summary>
        /// Accept socket.
        /// </summary>
        internal Socket Connection
        {
            get { return this.connection; }
        }

        /// <summary>
        /// Process data received from the client.
        /// </summary>
        /// <param name="args">SocketAsyncEventArgs used in the operation.</param>
        internal void ProcessData(SocketAsyncEventArgs args)
        {
            // Get the message received from the client.
            String received = this.sb.ToString();

            //TODO Use message received to perform a specific operation.
            Console.WriteLine("Received: \"{0}\". The server has read {1} bytes.", received, received.Length);

            Byte[] sendBuffer = Encoding.ASCII.GetBytes(received);
            args.SetBuffer(sendBuffer, 0, sendBuffer.Length);

            // Clear StringBuffer, so it can receive more data from a keep-alive connection client.
            sb.Length = 0;
            this.currentIndex = 0;
        }

        /// <summary>
        /// Set data received from the client.
        /// </summary>
        /// <param name="args">SocketAsyncEventArgs used in the operation.</param>
        internal void SetData(SocketAsyncEventArgs args)
        {
            Int32 count = args.BytesTransferred;

            if ((this.currentIndex + count) > this.sb.Capacity)
            {
                throw new ArgumentOutOfRangeException("count",
                    String.Format(CultureInfo.CurrentCulture, "Adding {0} bytes on buffer which has {1} bytes, the listener buffer will overflow.", count, this.currentIndex));
            }

            sb.Append(Encoding.ASCII.GetString(args.Buffer, args.Offset, count));
            this.currentIndex += count;
        }

        #region IDisposable Members

        /// <summary>
        /// Release instance.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.connection.Shutdown(SocketShutdown.Send);
            }
            catch (Exception)
            {
                // Throw if client has closed, so it is not necessary to catch.
            }
            finally
            {
                this.connection.Close();
            }
        }

        #endregion
    }
}
