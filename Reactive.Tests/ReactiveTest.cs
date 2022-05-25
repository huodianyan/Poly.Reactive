using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Poly.Reactive.Tests
{
    [TestClass]
    public partial class ReactiveTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
        }
        [ClassCleanup]
        public static void ClassCleanup()
        {
        }
        [TestInitialize]
        public void TestInitialize()
        {
        }
        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void SubjectTest()
        {
            //var tcs = new TaskCompletionSource<bool>();
            var observer1 = new TestObserver(1);
            var observer2 = new TestObserver(2);

            var source = new Subject<int>();
            var subscription1 = source.Subscribe(observer1);
            var subscription2 = source.Subscribe(observer2);

            source.OnNext(11);
            Assert.AreEqual(11, observer1.value);
            Assert.AreEqual(11, observer2.value);

            subscription1.Dispose();

            source.OnNext(12);
            Assert.AreNotEqual(12, observer1.value);
            Assert.AreEqual(12, observer2.value);

            subscription1 = source.Subscribe(observer1);
            source.OnNext(13);
            Assert.AreEqual(13, observer1.value);
            Assert.AreEqual(13, observer2.value);

            source.OnError(new Exception("Error"));
            Assert.AreEqual(-2, observer1.value);
            Assert.AreEqual(-2, observer2.value);

            source.Dispose();
        }

        public class TestObserver : IObserver<int>
        {
            internal int value;
            readonly int id;
            public TestObserver(int id) => this.id = id;
            public void OnCompleted()
            {
                value = -1;
                Console.WriteLine($"{id}: OnCompleted!");
            }
            public void OnError(Exception error)
            {
                value = -2;
                Console.WriteLine($"{id}: OnError!");
            }

            public void OnNext(int value)
            {
                var oldValue = this.value;
                this.value = value;
                Console.WriteLine($"{id}: OnNext {oldValue} -> {value}");
            }
        }
    }
}