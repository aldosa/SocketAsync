using System;
using System.IO;

namespace CircularBuffer
{
    public class CircularStream : Stream
    {
        private CircularBuffer<byte> buffer;

        public CircularStream(int bufferCapacity)
            : base()
        {
            buffer = new CircularBuffer<byte>(bufferCapacity);
        }

        public override long Position
        {
            get
            {
                return this.buffer.Head;
            }
            set
            {
                this.buffer.SetFirst();
                this.buffer.Skip((int)value);
            }
        }

        public int Capacity
        {
            get { return buffer.Capacity; }
            set { buffer.Capacity = value; }
        }

        public override long Length
        {
            get { return buffer.Size; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public byte[] GetBuffer()
        {
            return buffer.GetBuffer();
        }

        public byte[] ToArray()
        {
            return buffer.ToArray();
        }

        public override void Flush()
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.buffer.Put(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            this.buffer.Put(value);
        }
        
        /// <summary>
        /// Copies part of the buffer to a target buffer reference.
        /// </summary>
        /// <param name="targetBuffer">The reference to the target.</param>
        /// <param name="offsetInTarget">offset within the target, not the source.
        /// If you want to offset the source, use "Position" property</param>
        /// <param name="count">number of bytes to add</param>
        /// <returns>actual number of bytes copied</returns>
        public override int Read(byte[] targetBuffer, int offsetInTarget, int count)
        {
            return this.buffer.Get(targetBuffer, offsetInTarget, count);
        }

        public override int ReadByte()
        {
            return this.buffer.Get();
        }
        
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch(origin)
            { 
                case SeekOrigin.Begin:
                    this.buffer.SetFirst();
                    this.buffer.Skip((int)offset);
                    break;

                case SeekOrigin.Current:
                    this.buffer.Skip((int)offset);
                    break;
            
                case SeekOrigin.End:
                    this.buffer.SetLast();
                    //todo: offset must be negative!
                    this.buffer.Skip((int)offset);
                    break;

            }

            return -1; //?

        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
    }
}
