﻿using NUnit.Framework;
using Assert = GameFlow.Internal.Assert;

namespace GameFlow.Tests
{
    public class Event_Tests
    {
        public class TestScript___EventCallback1 : GameFlowElement
        {
            public static int timeRun;
        }

        public class TestScript___EventCallback2 : GameFlowElement
        {
            public static int timeRun;
        }

        [SetUp]
        public void SetUp()
        {
            GameFlowEvent.events = new EventCollection();
            TestScript___EventCallback1.timeRun = 0;
            TestScript___EventCallback2.timeRun = 0;
        }

        [Test]
        public void _0_Simple_Callback()
        {
            GameFlowEvent.Listen<TestScript___EventCallback1>(onActive: Test1);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), null, null);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), null, null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 2);
            GameFlowEvent.RemoveListener<TestScript___EventCallback1>(onActive: Test1);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), null, null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 2);
        }

        [Test]
        public void _1_Simple_Callback()
        {
            GameFlowEvent.Listen<TestScript___EventCallback1>(onActive: Test1);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), null, null);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback2), null, null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 1);
            Assert.IsTrue(TestScript___EventCallback2.timeRun == 0);
            GameFlowEvent.Listen<TestScript___EventCallback2>(onActive: Test2);
            GameFlowEvent.RemoveListener<TestScript___EventCallback1>(onActive: Test1);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), null, null);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback2), null, null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 1);
            Assert.IsTrue(TestScript___EventCallback2.timeRun == 1);
        }

        [Test]
        public void _2_Duplicate_2_Action()
        {
            GameFlowEvent.Listen<TestScript___EventCallback1>(onActive: Test1);
            GameFlowEvent.Listen<TestScript___EventCallback1>(onActive: Test2);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), null, null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 1);
            Assert.IsTrue(TestScript___EventCallback2.timeRun == 1);
        }

        [Test]
        public void _3_Simple_With_ID_Callback()
        {
            GameFlowEvent.Listen<TestScript___EventCallback1>(onActive: Test1, "null");
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), "null", null);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), "null", null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 2);
            GameFlowEvent.RemoveListener<TestScript___EventCallback1>(onActive: Test1, "null");
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), "null", null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 2);
        }

        [Test]
        public void _4_Simple_With_ID_Callback()
        {
            GameFlowEvent.Listen<TestScript___EventCallback1>(onActive: Test1, "null");
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), "null", null);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), "null", null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 2);
            GameFlowEvent.RemoveListener<TestScript___EventCallback1>(onActive: Test1);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), "null", null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 3);
        }

        [Test]
        public void _5_Simple_With_ID_Callback()
        {
            GameFlowEvent.Listen<TestScript___EventCallback1>(onActive: Test1);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), "null", null);
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), "null", null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 0, "1 TestScript___EventCallback1.timeRun = " + TestScript___EventCallback1.timeRun);
            GameFlowEvent.RemoveListener<TestScript___EventCallback1>(onActive: Test1, "null");
            GameFlowEvent.OnActive(typeof(TestScript___EventCallback1), "null", null);
            Assert.IsTrue(TestScript___EventCallback1.timeRun == 0, "2 TestScript___EventCallback1.timeRun = " + TestScript___EventCallback1.timeRun);
        }

        private void Test1(object data)
        {
            TestScript___EventCallback1.timeRun++;
        }

        private void Test2(object data)
        {
            TestScript___EventCallback2.timeRun++;
        }
    }
}