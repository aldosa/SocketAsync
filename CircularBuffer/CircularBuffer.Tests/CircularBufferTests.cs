using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CircularBuffer.Tests
{
    [TestClass()]
    public class CircularBufferTests
    {
        private static Random rnd;

        [ClassInitialize()]
        public static void Initialize(TestContext testContext)
        {
            rnd = new Random();
        }

        [ClassCleanup()]
        public static void Cleanup()
        {
        }

        protected static byte[] GenerateRandomData(int length)
        {
            var data = new byte[length];
            rnd.NextBytes(data);
            return data;
        }

        private TestContext testContext;

        public CircularBufferTests()
        {
        }
        
        public TestContext TestContext
        {
            get
            {
                return testContext;
            }
            set
            {
                testContext = value;
            }
        }
        
        [TestInitialize()]
        public void TestInitialize()
        {
        }

        [TestCleanup()]
        public void TestCleanup()
        {
        }

        [TestMethod()]
        public void EmptyBuffer()
        {
            var data = GenerateRandomData(100);
            var buffer = new CircularBuffer<byte>(data.Length);
            buffer.Put(data);
            var ret = new byte[buffer.Size];
            buffer.Get(ret);
            CollectionAssert.AreEqual(data, ret);
            Assert.IsTrue(buffer.Size == 0);
        }

        [TestMethod()]
        public void FillBuffer()
        {
            var data = GenerateRandomData(100);
            var buffer = new CircularBuffer<byte>(data.Length);
            buffer.Put(data);
            CollectionAssert.AreEqual(data, buffer.ToArray());
        }

        [TestMethod()]
        public void WrapAroundBuffer()
        {
            var data = GenerateRandomData(100);
            var buffer = new CircularBuffer<byte>(data.Length, true);
            buffer.Put(GenerateRandomData(data.Length / 2));
            buffer.Put(data);
            buffer.Skip(data.Length - data.Length / 2);
            CollectionAssert.AreEqual(data, buffer.ToArray());
        }

        [TestMethod(), ExpectedException(typeof(InvalidOperationException))]
        public void OverflowBuffer()
        {
            var data = GenerateRandomData(100);
            var buffer = new CircularBuffer<byte>(data.Length - 1, false);
            buffer.Put(data);
        }
    }
}
